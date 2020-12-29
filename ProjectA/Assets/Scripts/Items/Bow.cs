using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Inventory.InventorySlot;

namespace Items
{
    [Obsolete]
    public interface IDamageTaken
    {
        void OnDamageTaken(int damage, Vector2 src);
    }
    
    [LoadResourceToField("bullet", "Bullet", typeof(GameObject))]
    public class Bow : Weapon2
    {
        public class Charging : StatusEffect
        {
            Bow bow;

            public override void OnDamage(Entity currentEntity, int damage, out bool shouldRemove)
            {
                if (damage > 0)
                {
                    //cancel charge
                    if (bow.ChargingState)
                    {
                        bow.ChargingState = false;
                        bow.PlayerAnim.SetBool("isAiming", false);
                        Vector2 lookDir = Player.Instance.MousePositionRelative.normalized;
                        bow.PlayerAnim.SetFloat("xInput", lookDir.x);
                        bow.PlayerAnim.SetFloat("yInput", lookDir.y);
                    }

                    shouldRemove = true;
                    Debug.Log("Canceling charge");
                }
                else
                {
                    shouldRemove = false;
                }
            }

            public override void OnRemoval(Entity currentEntity)
            {

                //reset player speed
                bow.ChargingState = false;
            }

            public Charging(Bow bow)
            {
                this.bow = bow;
            }
        }

        static int bowMass = 1;
        public static readonly string itemName = "Bow";
        public override string Name => itemName;
        public static Sprite sprite;
        public override Sprite Sprite => sprite;
        public static GameObject bullet;

        public int baseDamage;
        public int chargedDamage;
        public float baseSpeed;
        public float knockback;
        public float fullCharge;
        public float fullChargeCooldown;
        public float slowMoveSpeedMultiplier;

        public int GetDamage(float t)
        {
            return t < fullCharge ? baseDamage : chargedDamage;
        }
        public float GetKnockback(float t)
        {
            return t < fullCharge ? knockback : 1.5f * knockback;
        }
        public float GetSpeed(float t)
        {
            return t < fullCharge ? baseSpeed : 1.5f * baseSpeed;
        }

        //TODO:Get rid of this
        //innitially a workaround but will remove
        private float defSpeed = 5;
        private FireProjectile fireArrow;

        //replacement to assignment at awake (since awake occurs on instantiation)
        private PlayerMove Movement => Player.Instance.playerMove;
        private Animator PlayerAnim => Player.Instance.playerAnimator;
        #region charging

        private Bow.Charging chargingEffect;
        private float chargeTime = 0;
        private bool _isCharging = false;
        public bool ChargingState 
        { 
            get => _isCharging;
            private set
            {
                if (value)
                {
                    chargeTime = Time.time;
                    Movement.defaultSpeed *= slowMoveSpeedMultiplier;
                }
                else
                {
                    Movement.defaultSpeed = defSpeed;
                }
                _isCharging = value;
            }
        }

        #endregion

        public override void Initialize()
        {
            //two handed
            this.SwapSlot = twoHanded;

            //initialize the status effect
            chargingEffect = new Charging(this);

            fireArrow = new FireProjectile(bullet, 0, 0, 0);
        }
        public override void AltFire(Transform player, bool down)
        {

        }
        public override void Fire(Transform player, bool down)
        {
            if (down)
            {
                if (!IsReady) return;

                //begin charge
                ChargingState = true;
                PlayerAnim.SetBool("isAiming", true);

                //apply the charging effect
                Player.Instance.ApplyEffect(chargingEffect);
            }
            else
            {
                if (!ChargingState) return;
                chargeTime = Time.time - this.chargeTime;

                //remove the charging effect
                //removing the effect will set player to default speed
                Player.Instance.EndEffect(chargingEffect);
                Debug.Log("Firing");

                //release and fire
                PlayerAnim.SetBool("isAiming", false);
                Player.Instance.PlaySound(useSound);
                Vector2 lookDir = (CameraReference.Instance.camera.ScreenToWorldPoint(Input.mousePosition) - Player.Instance.gameObject.transform.position).normalized;
                PlayerAnim.SetFloat("xInput", lookDir.x);
                PlayerAnim.SetFloat("yInput", lookDir.y);
                //chargeTime is deltaTime
                int damage = GetDamage(chargeTime);
                float kb = GetKnockback(chargeTime);
                float speed = GetSpeed(chargeTime);

                fireArrow.damage = damage;
                fireArrow.knockBack = kb;
                fireArrow.speed = speed;

                var i = fireArrow.Execute(player, out _);

                float cooldown = chargeTime < fullCharge ? cooldownTime : fullChargeCooldown;
                SetUseTime(cooldown);
            }
        }

        public Bow() : base(bowMass)
        {

        }
    }
}

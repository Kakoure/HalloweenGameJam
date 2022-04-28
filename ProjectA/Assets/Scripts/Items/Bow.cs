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
    
    //TODO: clean up this class
    [LoadResourceToField("bullet", "Bullet", typeof(GameObject))]
    public class Bow : Weapon
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
        public override AnimationControllerID AnimationControllerID => throw new NotImplementedException();

        public int baseDamage;
        public int chargedDamage;
        public float baseSpeed;
        public float knockback;
        public float fullChargeTime;
        public float standardChargeCooldown;
        public float fullChargeCooldown;
        public float slowMoveSpeedMultiplier;

        public int GetDamage(float t)
        {
            return t < fullChargeTime ? baseDamage : chargedDamage;
        }
        public float GetKnockback(float t)
        {
            return t < fullChargeTime ? knockback : 1.5f * knockback;
        }
        public float GetSpeed(float t)
        {
            return t < fullChargeTime ? baseSpeed : 1.5f * baseSpeed;
        }

        private FireProjectile fireArrow;

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
                    Movement.playerSpeed = slowMoveSpeedMultiplier * Movement.defaultSpeed;
                }
                else
                {
                    Movement.playerSpeed = Movement.defaultSpeed;
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
                if (!weaponCooldown.IsReady) return;

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

                //set damage, speed, and knockback for the projectile
                fireArrow.damage = GetDamage(chargeTime);
                fireArrow.knockBack = GetKnockback(chargeTime);
                fireArrow.speed = GetSpeed(chargeTime);

                //fire the projectile
                fireArrow.Execute(player, out _);

                //set the cooldown of the attack
                float cooldown = chargeTime < fullChargeTime ? standardChargeCooldown : fullChargeCooldown;
                weaponCooldown.Use(cooldown);
            }
        }

        public Bow() : base(bowMass)
        {

        }
    }
}

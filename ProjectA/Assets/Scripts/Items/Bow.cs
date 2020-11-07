using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Inventory.InventorySlot;

namespace Items
{
    public interface IDamageTaken
    {
        void OnDamageTaken(int damage, Vector2 src);
    }

    public class Bow : Weapon, IDamageTaken
    {
        static int bowMass = 1;
        public static readonly ItemID id = ItemID.Bow;
        public override ItemID ID => id;
        public static readonly string itemName = "Bow";
        public override string Name => itemName;
        public static Sprite projectileSprite;

        public Sprite arrowSprite;

        public int baseDamage;
        public int chargedDamage;
        public float baseSpeed;
        public float knockback;
        public float fullCharge;
        public float fullChargeCooldown;
        public float slowMoveSpeedMultiplier;

        private float defSpeed;
        public FireProjectile fireArrow; // start
        private PlayerMove movement;
        private Animator playerAnim;
        #region charging

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
                    movement.speed *= slowMoveSpeedMultiplier;
                }
                else
                {
                    movement.speed = defSpeed;
                }
                _isCharging = value;
            }
        }

        #endregion

        private Sprite _sprite;
        public override Sprite Sprite => _sprite;

        private void Start()
        {
            _sprite = GetComponent<SpriteRenderer>().sprite;
            movement = Player.Instance.GetComponent<PlayerMove>();
            defSpeed = movement.speed;
            playerAnim = Player.Instance.GetComponent<Animator>();
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
                playerAnim.SetBool("isAiming", true);
            }
            else
            {
                if (!ChargingState) return;

                chargeTime = Time.time - this.chargeTime;

                //release and fire
                ChargingState = false;
                playerAnim.SetBool("isAiming", false);
                Player.Instance.PlaySound(useSound);
                Vector2 lookDir = (CameraReference.Instance.camera.ScreenToWorldPoint(Input.mousePosition) - Player.Instance.gameObject.transform.position).normalized;
                playerAnim.SetFloat("xInput", lookDir.x);
                playerAnim.SetFloat("yInput", lookDir.y);
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

        public void OnDamageTaken(int damage, Vector2 src)
        {
            //cancel charge
            if (ChargingState)
            {
                ChargingState = false;
                playerAnim.SetBool("isAiming", false);
                Vector2 lookDir = (CameraReference.Instance.camera.ScreenToWorldPoint(Input.mousePosition) - Player.Instance.gameObject.transform.position).normalized;
                playerAnim.SetFloat("xInput", lookDir.x);
                playerAnim.SetFloat("yInput", lookDir.y);
            }
        }

        //occupy both slots
        internal override void SwapSlot(Inventory.InventorySlot currentSlot, Inventory.InventorySlot otherSlot, out bool success, out Action finalize)
        {
            finalize = null;
            success = true;

            // * -> null
            if (otherSlot == null)
            {
                switch (currentSlot.slotType)
                {
                    case SlotType.Weapon:
                    case SlotType.Shield:
                        // Shield || Weapon -> null
                        SlotType other = currentSlot.slotType == SlotType.Shield ? SlotType.Weapon : SlotType.Shield;
                        Inventory.InventorySlot blockerSlot = Inventory.GetSlot(other);
                        finalize = () => blockerSlot.Item = null;
                        return;
                    default:
                        // Inventory -> null
                        return;
                }
            }

            switch (otherSlot.slotType)
            {
                case SlotType.Inventory:
                    {
                        if (currentSlot == null || currentSlot.slotType == SlotType.Inventory)
                        {
                            //null || inventory -> inventory
                            success = true;
                            return;
                        }
                        else
                        {
                            //shield || weapon -> inventory
                            SlotType other = currentSlot.slotType == SlotType.Shield ? SlotType.Weapon : SlotType.Shield;
                            Inventory.InventorySlot secondSlot = Inventory.GetSlot(other);
                            Action removeBlocker = () =>
                            {
                                secondSlot.Item = null;
                            };
                            success = true;
                            finalize = removeBlocker;
                            return;
                        }
                    }
                case SlotType.Shield:
                case SlotType.Weapon:
                    {
                        SlotType other = otherSlot.slotType == SlotType.Shield ? SlotType.Weapon : SlotType.Shield;
                        Inventory.InventorySlot blocker = Inventory.GetSlot(other);

                        if (blocker.Item == this)
                        {
                            // Weapon -> Shield
                            success = false;
                            return;
                        }

                        if(blocker.Item != null)
                        {
                            //try to swap it out
                            int swapIndex = Inventory.GetEmptyInventorySlot();
                            if (swapIndex != -1)
                            {
                                Inventory.InventorySlot swapSlot = Inventory.GetSlot(SlotType.Inventory, swapIndex);
                                success = Inventory.Swap(blocker, swapSlot);
                                //fill the blocker slot if success
                                finalize = () => blocker.Item = this;
                                return;
                            }
                            else
                            {
                                //no swap slot avaliable
                                success = false;
                                return;
                            }
                        }
                        else
                        {
                            //blocker is empty
                            success = true;
                            Action fillBlocker = () =>
                            {
                                blocker.Item = this;
                            };
                            finalize = fillBlocker;
                            return;
                        }
                    }
            }
        }

        public Bow() : base(bowMass)
        {

        }
    }
}

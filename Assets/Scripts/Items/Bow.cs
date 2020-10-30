using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    public interface IDamageTaken
    {
        void OnDamageTaken(int damage, Vector2 src);
    }

    public class Bow : Weapon, IDamageTaken
    {
        static int bowMass = 1;

        public Sprite arrowSprite;

        public int baseDamage;
        public float baseSpeed;
        public float knockback;
        public float fullCharge;
        public float slowMoveSpeedMultiplier;

        private float defSpeed;
        public FireProjectile fireArrow;
        private PlayerMove movement;

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
                    chargeTime = Time.time - chargeTime;
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
            id = ItemID.Bow;
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
            }
            else
            {
                if (!ChargingState) return;

                //release and fire
                ChargingState = false;

                //chargeTime is deltaTime
                int damage = GetDamage(chargeTime);
                float kb = GetKnockback(chargeTime);
                float speed = GetSpeed(chargeTime);

                fireArrow.damage = damage;
                fireArrow.knockBack = kb;
                fireArrow.speed = speed;

                var i =fireArrow.Execute(player, out _);

                SetUseTime();
            }
        }

        public int GetDamage(float t)
        {
            return t < fullCharge ? baseDamage : 2 * baseDamage;
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
                ChargingState = false;
        }

        public Bow() : base(bowMass)
        {

        }
    }
}

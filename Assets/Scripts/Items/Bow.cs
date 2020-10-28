using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    public class Bow : Item
    {
        static int bowMass = 1;

        public Sprite arrowSprite;
        FireProjectile fireArrow;

        public int basedamage;
        public float baseSpeed;
        public float fullCharge;

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
                }
                else
                {
                    chargeTime = Time.time - chargeTime;
                }
                _isCharging = value;
            }
        }

        #endregion

        private Sprite _sprite;
        public override Sprite Sprite => _sprite;

        private void Awake()
        {
            _sprite = GetComponent<Sprite>();
        }

        public override void AltFire(Transform player, bool down)
        {

        }

        public override void Fire(Transform player, bool down)
        {
            if (down)
            {
                //begin charge
                ChargingState = true;
            }
            else
            {
                //release and fire
                ChargingState = false;

                //chargeTime is deltaTime
                int damage = GetDamage(chargeTime);
                float kb = GetKnockback(chargeTime);

                fireArrow.damage = damage;
                fireArrow.knockBack = kb;

                var bul = fireArrow.Execute(player, out var dir);

            }
        }

        public int GetDamage(float t)
        {
            //TODO:some mathematical fctn
            return 0;
        }
        public float GetKnockback(float t)
        {
            //TODO:some mathematical fctn
            return 0;
        }

        public Bow() : base(bowMass)
        {
            fireArrow = new FireProjectile(CameraReference.Instance.bulletGeneric, 0, 0, 0);
        }
    }
}

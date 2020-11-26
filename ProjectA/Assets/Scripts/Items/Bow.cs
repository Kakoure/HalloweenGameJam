﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    
    [LoadResourceToField("bullet", "Bullet", typeof(GameObject))]
    public class Bow : Weapon, IDamageTaken
    {
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

        //TODO FIX THIS
        //introduce status effects
        private float defSpeed = 5;
        private FireProjectile fireArrow;

        //replacement to assignment at awake (since awake occurs on instantiation)
        private PlayerMove movement => Player.Instance.playerMove;
        private Animator playerAnim => Player.Instance.playerAnimator;
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

        public override void Initialize()
        {
            //two handed
            this.SwapSlot = twoHanded;

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

        public Bow() : base(bowMass)
        {

        }
    }
}

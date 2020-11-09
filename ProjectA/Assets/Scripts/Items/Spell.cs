using Items;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class Spell : Weapon
    {
        public static ItemID itemID = ItemID.Staff;
        public override ItemID ID => itemID;
        public static string itemName = "Staff";
        public override string Name => itemName;

        static readonly int SpellMass = 1;

        public int damage;
        public float force;
        public int explosionDamage;
        public float explosionForce;
        public float explosionRadius;
        public float spd;
        public GameObject fireball;

        //serialized
        public FireProjectile fireProjectile;

        //assigned on load
        private Sprite sprite;
        public override Sprite Sprite => sprite;

        public override void AltFire(Transform player, bool down)
        {
            Fire(player, down);
        }

        public override void Fire(Transform player, bool down)
        {
            if (down)
            {
                if (!IsReady) return;

                //fire a projectile
                Fireball bullet = (Fireball)fireProjectile.Execute(player, out Vector2 dir);
                bullet.fuse = Time.time + dir.magnitude / spd;
                bullet.explosionDamage = explosionDamage;
                bullet.explosionForce = explosionForce;
                bullet.explosionRadius = explosionRadius;
                Player.Instance.PlaySound(useSound);

                SetUseTime();
            }
        }


        //delete
        private void Awake()
        {

        }

        public Spell() : base(SpellMass)
        {

        }
    }
}
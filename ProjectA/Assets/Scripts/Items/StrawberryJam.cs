using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class StrawberryJam : Item
    {
        public static readonly string itemName = "Jam";
        public override string Name => itemName;
        public static Sprite sprite;
        public override Sprite Sprite => sprite;
        public override AnimationControllerID AnimationControllerID => AnimationControllerID.unarmed;

        public static int itemMass = 1;

        public FireProjectile throwJar = new FireProjectile(null, itemMass, Mathf.Sqrt(massConstant / itemMass), .5f);
        public int healHP;

        //delete
        public void Start()
        {
            throwJar = new FireProjectile(null, itemMass, Mathf.Sqrt(massConstant / itemMass), .5f);
        }


        public override void AltFire(Transform player, bool down)
        {
            //Fire(player, down);

            if (down)
            {
                //should pass an instance to the entity that used it...
                Player.Instance.DealDamage(-healHP, 0, Vector2.zero);
                Inventory.Instance.shield.Item = null;
            }

        }
        public override void Fire(Transform player, bool down)
        {
            if (down)
            {
                Player.Instance.PlaySound(useSound);
                //should pass an instance to the entity that used it...
                Player.Instance.DealDamage(-healHP, 0, Vector2.zero);
                Inventory.Instance.weapon.Item = null;
            }
        }

        public StrawberryJam() : base(itemMass)
        {

        }
    }
}
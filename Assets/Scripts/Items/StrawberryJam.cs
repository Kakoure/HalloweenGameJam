using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class StrawberryJam : Item
    {
        public static readonly ItemID itemID = ItemID.Jam;
        public override ItemID ID => itemID;
        public static readonly string itemName;
        public override string Name => itemName;

        public static int itemMass = 1;

        public Sprite sprite;
        public override Sprite Sprite => sprite;
        public FireProjectile throwJar;
        public int healHP;

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

                Inventory.PopFromSlot(Inventory.Instance.shield);
            }

        }
        public override void Fire(Transform player, bool down)
        {
            if (down)
            {
                Player.Instance.PlaySound(useSound);
                //should pass an instance to the entity that used it...
                Player.Instance.DealDamage(-healHP, 0, Vector2.zero);

                Inventory.PopFromSlot(Inventory.Instance.weapon);
            }
        }

        public StrawberryJam() : base(itemMass)
        {

        }
    }
}
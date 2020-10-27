using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class StrawberryJam : Item
    {
        public static int itemMass = 1;

        public Sprite sprite;
        public override Sprite Sprite => sprite;

        public override void AltFire(Transform player)
        {
            throw new System.NotImplementedException();
        }
        public override void Fire(Transform player)
        {
            Debug.Log("You begin eating the Strawberry jam");
        }

        public StrawberryJam() : base(itemMass)
        {

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class StrawberryJam : Item
    {
        public static int itemMass = 1;
        
        [SerializeField]
        private Sprite sprite;
        public override Sprite Sprite => sprite;

        public override void AltFire()
        {
            throw new System.NotImplementedException();
        }
        public override void Apply(Item other)
        {
            throw new System.NotImplementedException();
        }
        public override void Eat(out int saturation)
        {
            throw new System.NotImplementedException();
        }
        public override void Fire()
        {
            throw new System.NotImplementedException();
        }
        public override void Throw(float momentum, ref int damage, Collision2D collision)
        {
            throw new System.NotImplementedException();
        }
        public override void UnWield(out bool success)
        {
            throw new System.NotImplementedException();
        }
        public override void Wield(out bool success)
        {
            throw new System.NotImplementedException();
        }

        public StrawberryJam() : base(itemMass)
        {

        }
    }
}
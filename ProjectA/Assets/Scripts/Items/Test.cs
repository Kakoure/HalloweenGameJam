using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    class Test : Item
    {
        public static int mass = 1;

        public static Sprite sprite;
        public override Sprite Sprite => sprite;

        public static string itemName = "Test";
        public override string Name => itemName;

        // OPTIONAL: add more static fields

        // space bar
        public override void AltFire(Transform player, bool down)
        {
            throw new NotImplementedException();
        }

        // left click
        public override void Fire(Transform player, bool down)
        {
            Debug.Log("Hi");
        }

        // OPTIONAL: add more overrides

        //constructor
        public Test() : base(mass) { }
    }
}

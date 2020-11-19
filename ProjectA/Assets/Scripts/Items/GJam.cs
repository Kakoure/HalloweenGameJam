using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    //this is how you create a reskin
    class GJam : StrawberryJam
    {
        public static int mass = 1;

        public static new Sprite sprite;
        public override Sprite Sprite => sprite;

        public static new string itemName = "Golden Jam";
        public override string Name => itemName;

        //constructor
        public GJam() { }
    }
}

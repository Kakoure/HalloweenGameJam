using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    public static class ItemIDExtend
    {
        public static int Value(this ItemID id)
        {
            return (int)id;
        }
    }

    public enum ItemID //So There is a single place to find and set id values
    {
        Error = -1,
        Unarmed = 0,
        Sword,
        Bow,
        Staff,
        Jam,
        SIZE, //i just like to do this
    }

    [LoadResourceToFieldInherited("sprite", "Sprite", typeof(Sprite))]
    public abstract partial class Item : ScriptableObject, IClickable
    {
        public static readonly int massConstant = 100;
        public static readonly float kbConst = 1.5f;

        public abstract Sprite Sprite { get; }
        public abstract ItemID ID { get; }
        public abstract string Name { get; }
        //since Item no longer inherits from Monobehavior, Owner needs to be used as an alternamtive
        public ItemGeneric Owner { get; set; } = null;
        public int Mass { get; protected set; } //letting mass be a modifiable value
        public AudioClip useSound;
        //this looks like it should be readonly static as opposed to being assigned on start

        public Item(int mass)
        {
            this.Mass = mass;
        }
    }
}

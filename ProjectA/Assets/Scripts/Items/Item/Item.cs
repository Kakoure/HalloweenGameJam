using JetBrains.Annotations;
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
        public static int Value(this ItemIDObsolete id)
        {
            return (int)id;
        }
    }

    //getting rid of this
    [Obsolete]
    public enum ItemIDObsolete //So There is a single place to find and set id values
    {
        Error = -1,
        Unarmed = 0,
        Sword,
        Bow,
        Staff,
        Jam,
        SIZE, //i just like to do this
    }

    public struct ItemID
    {
        public static readonly ItemID ERROR = -1;
        //since default value is zero, zero should have a special value
        public static readonly ItemID empty = 0;

        public static implicit operator ItemID(int val)
        {
            return new ItemID(val);
        }

        public static ItemID GetID<item>() where item : Item
        {
            return GetID(typeof(item));
        }
        public static ItemID GetID(Type item)
        {
            Array.FindIndex<Type>(Item.itemList, t => t == item)
        }

        int id;
        public int ID => id;

        public Type GetItemType()
        {
            throw new NotImplementedException();
        }

        public ItemID(int val)
        {
            this.id = val;
        }
    }

    [LoadResourceToFieldInherited("sprite", "Sprite", typeof(Sprite))]
    public abstract partial class Item : ScriptableObject, IClickable
    {
        public static readonly int massConstant = 100;
        public static readonly float kbConst = 1.5f;

        public abstract Sprite Sprite { get; }
        [Obsolete]
        public abstract ItemIDObsolete ID { get; }
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

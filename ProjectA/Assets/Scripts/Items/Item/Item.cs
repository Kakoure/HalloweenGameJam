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
        SIZE,
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
        public static implicit operator int(ItemID id)
        {
            return id.ID;
        }
        public static bool operator ==(ItemID thisId, ItemID otherId)
        {
            return thisId.ID == otherId.ID;
        }
        public static bool operator !=(ItemID thisId, ItemID otherId)
        {
            return thisId.ID != otherId.ID;
        }

        public static ItemID GetID<item>() where item : Item
        {
            return GetID(typeof(item));
        }
        public static ItemID GetID(Type item)
        {
            int idx = Array.FindIndex<Type>(Item.itemList, t => t == item);
            return new ItemID(idx + 1); //shift by 1 so that 0 has a unique value
        }

        //immutable i guess
        public int ID { get; }

        public Type GetItemType()
        {
            return Item.itemList[ID - 1]; // shift by 1
        }
        public string GetItemName()
        {
            return Item.itemNames[ID - 1]; //shift by 1
        }
        public override bool Equals(object obj)
        {
            if (obj is ItemID) return this == (ItemID)obj;
            else return false;
        }
        public override int GetHashCode()
        {
            return ID;
        }

        public ItemID(int val)
        {
            this.ID = val;
        }
    }

    [LoadResourceToFieldInherited("sprite", "Sprite", typeof(Sprite))]
    [System.Serializable]
    public abstract partial class Item : ScriptableObject, IClickable
    {
        public static readonly int massConstant = 100;
        public static readonly float kbConst = 1.5f;

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

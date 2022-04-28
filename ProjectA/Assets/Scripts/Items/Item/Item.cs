using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Inventory;
using static Inventory.InventorySlot;

namespace Items
{
    //getting rid of this
    [Obsolete]
    public enum ItemIDObsolete
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
        public static readonly ItemID EMPTY = 0;

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
            //TODO: use binary search instead
            int idx = Array.FindIndex(Item.itemList, t => t == item);
            return new ItemID(idx + 1); //shift by 1 so that 0 has a unique value
        }
        //does null checking
        public static ItemID GetID(Item item)
        {
            if (item == null) return EMPTY;
            else return GetID(item.GetType());
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

        /// <summary>
        /// determines how an item behaves when managing the inventory
        /// </summary>
        public delegate SlotController SlotBehaviour(Item thisItem);
        public delegate void SlotController(InventorySlot currentSlot, InventorySlot otherSlot, out bool success, out Action finalize);
        //behaves as a normal item
        public static readonly SlotBehaviour defaultSlotBehaviour = (thisItem) => (InventorySlot s1, InventorySlot s2, out bool s, out Action f) =>
        {
            s = true;

            //null behaves the way that you would expect
            f = null;
        };
        //behaves like a two handed item
        public static readonly SlotBehaviour twoHanded = (Item thisItem) => (InventorySlot s1, InventorySlot s2, out bool s, out Action f) =>
        {
            //pure spagetii ahead

            f = null;
            s = true;

            // * -> null
            if (s2 == null)
            {
                switch (s1.slotType)
                {
                    case SlotType.Weapon:
                    case SlotType.Shield:
                        // Shield || Weapon -> null
                        SlotType other = s1.slotType == SlotType.Shield ? SlotType.Weapon : SlotType.Shield;
                        Inventory.InventorySlot blockerSlot = Inventory.GetSlot(other);
                        f = () => blockerSlot.Item = null;
                        return;
                    default:
                        // Inventory -> null
                        return;
                }
            }

            switch (s2.slotType)
            {
                case SlotType.Inventory:
                    {
                        if (s1 == null || s1.slotType == SlotType.Inventory)
                        {
                            //null || inventory -> inventory
                            s = true;
                            return;
                        }
                        else
                        {
                            //shield || weapon -> inventory
                            SlotType other = s1.slotType == SlotType.Shield ? SlotType.Weapon : SlotType.Shield;
                            InventorySlot secondSlot = GetSlot(other);
                            s = true;
                            f = () => secondSlot.Item = null;
                            return;
                        }
                    }
                case SlotType.Shield:
                case SlotType.Weapon:
                    {
                        SlotType other = s2.slotType == SlotType.Shield ? SlotType.Weapon : SlotType.Shield;
                        InventorySlot blocker = GetSlot(other);

                        if (blocker.Item == thisItem)
                        {
                            // Weapon -> Shield
                            s = false;
                            return;
                        }

                        if (blocker.Item != null)
                        {
                            //try to swap it out
                            int swapIndex = Inventory.GetEmptyInventorySlot();
                            if (swapIndex != -1)
                            {
                                Inventory.InventorySlot swapSlot = Inventory.GetSlot(SlotType.Inventory, swapIndex);
                                s = Inventory.Swap(blocker, swapSlot);
                                //fill the blocker slot if success
                                f = () => blocker.Item = thisItem;
                                return;
                            }
                            else
                            {
                                //no swap slot avaliable
                                s = false;
                                return;
                            }
                        }
                        else
                        {
                            //blocker is empty
                            s = true;
                            Action fillBlocker = () =>
                            {
                                blocker.Item = thisItem;
                            };
                            f = fillBlocker;
                            return;
                        }
                    }
            }
        };

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

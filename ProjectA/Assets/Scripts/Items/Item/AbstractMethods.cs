using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static Inventory;
using static Inventory.InventorySlot;

namespace Items
{
    partial class Item
    {
        //determines how an item behaves when managing the inventory
        public delegate SlotController SlotBehaviour(Item thisItem);
        public delegate void SlotController(InventorySlot currentSlot, InventorySlot otherSlot, out bool success, out Action finalize);
        //behaves as a normal item
        public static readonly SlotBehaviour defaultSlotBehaviour = (thisItem) => (InventorySlot s1, InventorySlot s2, out bool s, out Action f) =>
        {
            s = true;

            //null behaves the way that you would expect
            f = null; 
        };
        //behaves like a two - handed item
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

        //abstract
        /// <summary>
        /// effect when using the item in the primary slot
        /// </summary>
        /// <param name="player"></param>
        /// <param name="down">true if mouse button down</param>
        public abstract void Fire(Transform player, bool down);
        /// <summary>
        /// effect when using the item in the shield slot
        /// </summary>
        /// <param name="player"></param>
        /// <param name="down">true if mouse button down</param>
        public abstract void AltFire(Transform player, bool down);
        /// <summary>
        /// outs true if the item is able to be dropped.
        /// out false to prevent the item from being dropped
        /// </summary>
        /// <param name="success"></param>
        protected virtual void DropItem(out bool success)
        {
            success = true;
        }
        /// <summary>
        /// behaves the same way that Start() behaves
        /// </summary>
        public virtual void Initialize()
        {

        }
        /// <summary>
        /// if both items out true then swap will occur and finalize will execute.
        /// </summary>
        /// <param name="currentSlot">can be null. If currentSlot is null then player is picking up the item</param>
        /// <param name="otherSlot">can be null. If otherSlot is null then player is dropping the item</param>
        /// <param name="success">true if the swap was successful</param>
        /// <param name="finalize">action when swap was successful</param>
        //in order to make this code more flexable, I should use a function of some sort in place of this messy thing
        //also neither parameter should ever be null
        /*
        internal virtual void SwapSlot(Inventory.InventorySlot currentSlot, Inventory.InventorySlot otherSlot, out bool success, out Action finalize)
        {
            finalize = null;
            success = true;
        }
        */
        public SlotBehaviour SwapSlot { get; protected set; } = defaultSlotBehaviour;
    }
}

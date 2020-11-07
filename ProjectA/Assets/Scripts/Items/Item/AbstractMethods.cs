using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    partial class Item
    {
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
        /// if both items out true then swap will occur and finalize will execute.
        /// </summary>
        /// <param name="currentSlot">can be null. If currentSlot is null then player is picking up the item</param>
        /// <param name="otherSlot">can be null. If otherSlot is null then player is dropping the item</param>
        /// <param name="success">true if the swap was successful</param>
        /// <param name="finalize">action when swap was successful</param>
        //in order to make this code more flexable, I should use a function of some sort in place of this messy thing
        //also neither parameter should ever be null
        internal virtual void SwapSlot(Inventory.InventorySlot currentSlot, Inventory.InventorySlot otherSlot, out bool success, out Action finalize)
        {
            finalize = null;
            success = true;
        }
    }
}

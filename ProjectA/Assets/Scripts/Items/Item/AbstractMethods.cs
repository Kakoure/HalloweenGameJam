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
        //abstract
        public abstract Sprite Sprite { get; }
        public abstract string Name { get; }
        public abstract AnimationControllerID AnimationControllerID { get; }

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
        public virtual SlotBehaviour SwapSlot { get; protected set; } = defaultSlotBehaviour;
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    public partial class Item
    {
        [Obsolete]
        public static Item InstantiateItem(Type itemType)
        {
            if (itemType.IsAbstract || !typeof(Item).IsAssignableFrom(itemType)) return null;
            else
            {
                //find a no arg constructor
                //im assuming that no arg is the only valid constructor
                var ctor = itemType.GetConstructor(new Type[] { });

                //invoke no arg constructor
                return (Item)ctor.Invoke(new object[] { });
            }
        }

        public void DropAt(Vector2 position)
        {
            DropItem(out bool success);
            if (!success) return;
            Owner.transform.position = position;
            MakeVisible();
        }

        //pick up the item
        public void OnClick()
        {
            Inventory.InventorySlot slot = Inventory.GetOpenSlot();

            //tries to put item in inventory slot
            bool success = Inventory.Pickup(slot, this);

            if (success)
            {
                //make the item disappear if successful
                MakeInvisible();
            }
            else
            {
                //do nothing
            }
        }

        //To remove sprite + collider without deactivating object (so I can use Corutines within items)
        private void MakeInvisible()
        {
            Owner.GetComponent<SpriteRenderer>().enabled = false;
            foreach (Collider2D col in Owner.GetComponents<Collider2D>())
            {
                col.enabled = false;
            }
        }
        private void MakeVisible()
        {
            Owner.GetComponent<SpriteRenderer>().enabled = true;
            foreach (Collider2D col in Owner.GetComponents<Collider2D>())
            {
                col.enabled = true;
            }
        }
    }
}

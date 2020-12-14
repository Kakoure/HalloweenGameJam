using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    public partial class Item
    {
        /// <summary>
        /// Creates a new Instance of an item. This must be called on an item if it's game object does not yet exist
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static Item InstantiateItem(Item original)
        {
            if (original == null) return null;

            Item copy = Instantiate<Item>(original);
            GameObject obj = GameObject.Instantiate(CameraReference.Instance.itemGeneric);

            //assign the copy before it reaches Start()
            obj.GetComponent<ItemGeneric>().itemObject = copy;

            return copy;
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

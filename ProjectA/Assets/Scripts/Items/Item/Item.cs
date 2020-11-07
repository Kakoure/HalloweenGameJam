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
        Unarmed,
        Sword,
        Bow,
        Staff,
        Jam,
        SIZE, //i just like to do this
    }

    //The Item class defines general 
    [Serializable]
    public abstract partial class Item : MonoBehaviour, IClickable
    {
        public static readonly int massConstant = 100;
        public static readonly float kbConst = 1.5f;

        public abstract Sprite Sprite { get; }
        public abstract ItemID ID { get; }
        public abstract string Name { get; }
        public Entity Owner { get; protected set; } = null;
        public int Mass { get; protected set; } //letting mass be a modifiable value
        /*
        public int ID 
        { 
            get => (int)id; 
            protected set => id = (ItemID)value;
        }
        protected ItemID id;
        */
        public AudioClip useSound;
        //this looks like it should be readonly static as opposed to being assigned on start

        public Item(int mass)
        {
            this.Mass = mass;
        }

        public void DropAt(Vector2 position)
        {
            DropItem(out bool success);
            if (!success) return;
            transform.position = position;
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
            GetComponent<SpriteRenderer>().enabled = false;
            foreach (Collider2D col in GetComponents<Collider2D>())
            {
                col.enabled = false;
            }
        }
        private void MakeVisible()
        {
            GetComponent<SpriteRenderer>().enabled = true;
            foreach (Collider2D col in GetComponents<Collider2D>())
            {
                col.enabled = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    public enum ItemID //So There is a single place to find and set id values
    {
        Unarmed,
        Sword,
        Bow,
        Staff,
        Jam

    }

    //The Item class defines general 
    [Serializable]
    public abstract class Item : MonoBehaviour , IClickable
    {
        public static readonly int massConstant = 100;
        public static readonly float kbConst = 1.5f;

        public abstract Sprite Sprite { get; }
        public Entity Owner { get; protected set; } = null;
        public int Mass { get; protected set; }
        public int ID { get { return (int)id; } protected set { id = (ItemID)value; } }
        protected ItemID id;
        public Item(int mass)
        {
            this.Mass = mass;
        }

        public void DropAt(Vector3 position)
        {
            DropItem(out bool success);
            if (!success) return;
            position.z = 0;
            transform.position = position;
            gameObject.SetActive(true);
        }


        //abstract
        /*
        public abstract void Eat(out int saturation);
        public abstract void Apply(Item other);
        public abstract void Throw(float momentum, ref int damage, Collision2D collision);
        public abstract void Wield(out bool success);
        public abstract void UnWield(out bool success);
        */
        public abstract void Fire(Transform player, bool down);
        public abstract void AltFire(Transform player, bool down);
        protected virtual void DropItem(out bool success)
        {
            success = true;
        }

        void IClickable.OnClick()
        {
            //pick up the item
            int slot = Inventory.GetOpenSlot();

            //check to see if theres an open slot
            if (slot != -1)
            {
                //make the item disappear
                gameObject.SetActive(false);

                //put the item in inventory
                Inventory.AssignTo(this, slot);
            }
            else
            {

            }

        }
    }
}

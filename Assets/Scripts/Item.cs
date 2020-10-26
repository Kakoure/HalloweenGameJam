using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    //The Item class defines general 
    [Serializable]
    public abstract class Item : MonoBehaviour , IClickable
    {
        public Entity Owner { get; protected set; } = null;
        public abstract Sprite Sprite { get; }
        public int Mass { get; protected set; }

        public Item(int mass)
        {
            this.Mass = mass;
        }

        //abstract
        public abstract void Eat(out int saturation);
        public abstract void Apply(Item other);
        public abstract void Throw(float momentum, ref int damage, Collision2D collision);
        public abstract void Wield(out bool success);
        public abstract void UnWield(out bool success);
        public abstract void Fire();
        public abstract void AltFire();

        void IClickable.OnClick()
        {
            //TODO: pick up the item

            Debug.Log("Item clicked");

            //make the item disappear
            gameObject.SetActive(false);

            //put the item in inventory
            Inventory.AssignTo(this, 0);

            //throw new NotImplementedException();
        }
    }
}

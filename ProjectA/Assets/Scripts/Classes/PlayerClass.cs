using Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayerClasses
{
    [CreateAssetMenu(menuName = "New Player Class")]
    public partial class PlayerClass : ScriptableObject
    {
        [Tooltip("(This is just a placeholder right now)")]
        public readonly Sprite classSprite;

        [SerializeField]
        private Item startingWeaponSlot;
        public Item Weapon { get => startingWeaponSlot; }
        [SerializeField]
        private Item startingOffhandSlot;
        public Item Offhand { get => startingWeaponSlot; }
        [SerializeField]
        private Item[] items;
        public ReadOnlyCollection<Item> Items { get => Array.AsReadOnly(items); }

        [Obsolete]
        public void InstantiateItems()
        {
            //instantiate the weapon but dont reassign the field
            //startingWeaponSlot = Item.InstantiateItem(startingWeaponSlot);
            //startingOffhandSlot = Item.InstantiateItem(startingOffhandSlot);

            /*
            for(int i = 0; i < items.Length; i++)
            {
                items[i] = Item.InstantiateItem(items[i]);
            }
            */
        }
    }
}

using Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayerClasses
{
    [CreateAssetMenu(menuName = "New Player Class")]
    public partial class PlayerClass : ScriptableObject
    {
        [Tooltip("(This is just a placeholder right now)")]
        public Sprite classSprite;

        [SerializeField]
        public Item startingWeaponSlot;
        [SerializeField]
        public Item startingOffhandSlot;
        [SerializeField]
        public Item[] items;

        public void InstantiateItems()
        {
            startingWeaponSlot = Item.InstantiateItem(startingWeaponSlot);
            startingOffhandSlot = Item.InstantiateItem(startingOffhandSlot);

            for(int i = 0; i < items.Length; i++)
            {
                items[i] = Item.InstantiateItem(items[i]);
            }
        }
    }
}

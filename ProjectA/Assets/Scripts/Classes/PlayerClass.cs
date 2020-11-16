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

        public Item startingWeaponSlot;
        public Item startingOffhandSlot;
        public Item[] items;
    }
}

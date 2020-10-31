using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    public abstract class Weapon : Item
    {
        public float cooldownTime;
        private float nextUse;

        public bool IsReady => Time.time > nextUse;
        protected void SetUseTime() => nextUse = Time.time + cooldownTime;
        protected void SetUseTime(float cooldown)
        {
            cooldownTime = cooldown;
            SetUseTime();
        }
        public float TimeRemaining() => nextUse - Time.time;

        public Weapon(int mass) : base(mass) { }
    }
}

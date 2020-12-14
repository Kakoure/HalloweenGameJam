using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    [Obsolete]
    public abstract class Weapon2 : Item
    {
        //all of this can be replaced by using a cooldown object
        public float cooldownTime;
        private float nextUse;

        public bool IsReady => Time.time > nextUse;
        protected void SetUseTime() => nextUse = Time.time + cooldownTime;
        protected void SetUseTime(float cooldown)
        {
            nextUse = Time.time + cooldownTime;
        }
        public float TimeRemaining() => nextUse - Time.time;

        public Weapon2(int mass) : base(mass) { }
    }
    
    public abstract class Weapon : Item
    {
        public delegate Action<Transform, bool> WeaponBehaviour(Item thisItem);
    
        //turn a virtual function into an exposed field
        protected virtual WeaponBehaviour primaryBehaviour { get; }
        public override void Fire(Transform player, bool down)
        {
            primaryBehaviour?.Invoke(this)(player, down);
        }

        protected virtual WeaponBehaviour offhandBehaviour { get; }
        public override void AltFire(Transform player, bool down)
        {
            offhandBehaviour?.Invoke(this)(player, down);
        }

        public Weapon(int mass) : base(mass) { }
    }
}

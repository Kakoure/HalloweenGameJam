using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    abstract class Item
    {
        public Entity owner;
        public string spriteDir;

        public abstract int Mass { get; }
        public abstract int ThrownDamage { get; }

        public abstract void Eat(out int saturation);
        public abstract void Apply(Item other);
        public abstract void Throw(float momentum, ref int damage, Collision2D collision);
        public abstract void Wield(out bool success);
        public abstract void UnWield(out bool success);
        public abstract void Strike(ref bool hit, ref int damage);
    }
}

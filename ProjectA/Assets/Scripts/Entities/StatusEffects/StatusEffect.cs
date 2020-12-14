using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Entities
{
    public static class Extensions
    { 
        public static bool OnDamage(this StatusEffect e, Entity currentEntity, int damage)
        {
            if (e == null)
            {
                Debug.LogError("null status effect found");
                return false;
            }
            e.OnDamage(currentEntity, damage, out bool b);
            return b;
        }
        public static bool OnUpdate(this StatusEffect e, Entity currentEntity)
        {
            if (e == null)
            {
                Debug.LogError("null status effect found");
                return false;
            }

            e.OnUpdate(currentEntity, out bool b);
            return b;
        }
    }
    public abstract class StatusEffect
    {
        /// <summary>
        /// Called when the StatusEffect is applied
        /// </summary>
        /// <param name="currentEntity"></param>
        public virtual void OnApplication(Entity currentEntity) { }
        /// <summary>
        /// triggered when the entity takes damage
        /// </summary>
        /// <param name="damage"></param>
        public virtual void OnDamage(Entity currentEntity, int damage, out bool shouldRemove) { shouldRemove = false; }
        /// <summary>
        /// triggered once per frame update
        /// </summary>
        public virtual void OnUpdate(Entity currentEntity, out bool shouldRemove) { shouldRemove = false; }
        /// <summary>
        /// triggered when the effect is removed
        /// </summary>
        public virtual void OnRemoval(Entity currentEntity) { }
    }
}

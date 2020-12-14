using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Entities
{
    public abstract class DurationEffect : StatusEffect
    {
        protected float endTime;
        public float Duration { get; protected set; }

        public override void OnUpdate(Entity currentEntity, out bool shouldRemove)
        {
            if (Time.time > endTime) shouldRemove = true;
            else shouldRemove = false;
        }

        public DurationEffect(float duration)
        {
            endTime = Time.time + duration;
            Duration = duration;
        }
    }
}

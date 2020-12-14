using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Entities
{
    partial class Entity
    {
        //status effects that might, for example, do somthing when the player takes damage.
        public LinkedList<StatusEffect> currentStatusEffects = new LinkedList<StatusEffect>();

        public void IterateStatusEffects(Func<StatusEffect, bool> action)
        {
            var currentNode = currentStatusEffects.First;
            while (currentNode != null)
            {
                var nextNode = currentNode.Next;
                bool remove = action(currentNode.Value);
                if (remove)
                {
                    currentStatusEffects.Remove(currentNode);

                    //call the event
                    currentNode.Value.OnRemoval(this);
                }
                currentNode = nextNode;
            }
        }

        public void ApplyEffect(StatusEffect effect)
        {
            if (effect == null) Debug.LogError("null effect found");

            //application event
            effect.OnApplication(this);

            currentStatusEffects.AddLast(effect);
        }
        public void EndEffect(StatusEffect effect)
        {
            currentStatusEffects.Remove(effect);

            //removal event
            effect.OnRemoval(this);
        }
        //does not call StatusEffect.OnRemove();
        public void DeleteEffect(StatusEffect effect)
        {
            currentStatusEffects.Remove(effect);

            //does not call StatusEffect.OnRemove();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    //tmp
    abstract public class Entity : MonoBehaviour
    {
        // TODO: ...
        public int HP;
        public abstract Rigidbody2D Rigidbody { get; }

        public void DealDamage(int damage)
        {
            HP -= damage;
            CameraReference.Instance.InstantiateHitMarker(damage, transform.position);
        }
        public void ApplyImpulse(float force, Vector2 from)
        {
            Vector2 disp = (Vector2)transform.position - from;
            Rigidbody.AddForce(disp.normalized * force, ForceMode2D.Impulse);
        }
        //etc.
    }
}

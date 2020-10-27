using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    //tmp
    public abstract class Entity : MonoBehaviour
    {
        // TODO: ...
        public int HP;
        public abstract Rigidbody2D Rigidbody { get; }
        public HealthBar HealthBar { get; private set; }

        public void DealDamage(int damage)
        {
            //updates the healthbar
            HP -= damage;
            HealthBar.Health = HP;
            CameraReference.Instance.InstantiateHitMarker(damage, transform.position);
            if (HP <= 0) Die();
        }
        public void Die()
        {
            if (HP <= 0)
                gameObject.SetActive(false);
        }
        public void ApplyImpulse(float force, Vector2 from)
        {
            Vector2 disp = (Vector2)transform.position - from;
            Rigidbody.AddForce(disp.normalized * force, ForceMode2D.Impulse);
        }

        public virtual void Awake()
        {
            HealthBar = GetComponent<HealthBar>();
            HealthBar.Health = HP;
        }
        //etc.
    }
}

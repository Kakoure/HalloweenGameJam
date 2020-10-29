using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace Items
{
    //tmp
    public abstract class Entity : MonoBehaviour
    {
        public int MaxHP;
        public int HP;
        public abstract Rigidbody2D Rigidbody { get; }
        public HealthBar healthBar;

        //return success
        public virtual bool DealDamage(int damage, float force, Vector2 from)
        {
            //updates the healthbar
            HP -= damage;
            healthBar.SetHealth(HP, MaxHP);
            CameraReference.Instance.InstantiateHitMarker(damage, transform.position);
            if (HP <= 0) {
                Die();
            }
            else {
                StartCoroutine("DamageFlash");
            }
            //apply impulse
            ApplyImpulse(force, from);

            return true;
        }
        public virtual void Die()
        {
            if (HP <= 0)
                gameObject.SetActive(false);
        }
        protected virtual void ApplyImpulse(float force, Vector2 from)
        {
            Vector2 disp = (Vector2)transform.position - from;
            Rigidbody.AddForce(disp.normalized * force, ForceMode2D.Impulse);
        }

        public virtual void Awake() { }
        public virtual void Start()
        {
            healthBar.SetHealth(HP, MaxHP);
        }

        private IEnumerator DamageFlash()
        {
            if(GetComponent<SpriteRenderer>() != null)
            {
                SpriteRenderer sprRend = GetComponent<SpriteRenderer>();
                sprRend.material.SetColor("_FlashColor", Color.white);
                sprRend.material.SetFloat("_FlashAmount", 1);
                yield return new WaitForSeconds(.1f);
                sprRend.material.SetColor("_FlashColor", Color.red);
                yield return new WaitForSeconds(.1f);
                sprRend.material.SetFloat("_FlashAmount", 0);
            }
           
        }
        //etc.
    }
}

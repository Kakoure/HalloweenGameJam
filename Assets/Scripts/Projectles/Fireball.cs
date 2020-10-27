using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Bullet
{
    [NonSerialized]
    public int explosionDamage = 0;
    [NonSerialized]
    public float explosionRadius = 0;
    [NonSerialized]
    public float explosionForce = 0;
    [NonSerialized]
    public float fuse = 1;

    private void Update()
    {
        if (Time.time > fuse)
            KillObject();
    }

    public Fireball()
    {
        this.onCollision = () =>
        {
            //explode, deal damage, addforce
            //TODO: also fire particles
            var collisions = Physics2D.OverlapCircleAll(this.transform.position, explosionRadius);
            foreach (var col in collisions)
            {
                if (col.gameObject.CompareTag("Monster"))
                {
                    Entity entity = col.GetComponent<Entity>();
                    entity.DealDamage(explosionDamage);
                    entity.ApplyImpulse(explosionForce, this.transform.position);
                }
            }
        };
    }
}

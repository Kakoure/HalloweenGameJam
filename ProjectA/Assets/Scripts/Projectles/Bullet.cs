using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    public Action onCollision = () => { };
    public float maxTime = Mathf.Infinity;
    public float knockBack = 0;

    #region IKillable Implimentation
    public bool IsDead
    {
        get
        {
            return !gameObject.activeSelf;
        }
    }
    public virtual void KillObject()
    {
        onCollision();
        GameObject.Destroy(this.gameObject);
    }
    #endregion
    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Time.time > maxTime) KillObject();
    }

    //rewritten to generalize with all shootable targets.
    private void OnTriggerStay2D(Collider2D collision)
    {
        //bullet should be on the same layer as player so no self collision
        //if (collision.gameObject == Owner) return;

        Entity entity = collision.gameObject.GetComponent<Entity>();
        if (entity != null)
        {
            bool damageSuccess = entity.DealDamage(Damage, knockBack, this.transform.position);
            if (damageSuccess)
            {
                KillObject();
            }
        }

        //delete on collision with wall
        //only if transform is inside the wall to make it easier to shoot through/around corners
        if (collision.gameObject.CompareTag("Geometry") && collision.OverlapPoint(this.transform.position))
        {
            KillObject();
        }
    }
    #endregion
}

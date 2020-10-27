using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
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
        GameObject.Destroy(this.gameObject);
    }
    #endregion
    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //rewritten to generalize with all shootable targets.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //bullet should be on the same layer as player so no self collision
        //if (collision.gameObject == Owner) return;

        if (collision.gameObject.CompareTag("Monster"))
        {
            Entity entity = collision.gameObject.GetComponent<Entity>();
            if (entity != null)
            {
                Vector2 pos = collision.ClosestPoint(gameObject.transform.position);
                entity.DealDamage(Damage);
                KillObject();
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //delete on collision with wall
        if (collision.gameObject.CompareTag("Geometry") && collision.OverlapPoint(this.transform.position))
        {
            KillObject();
        }
    }
    #endregion
}

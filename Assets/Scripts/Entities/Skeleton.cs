using CooldownTimer;
using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Entity
{
    private Rigidbody2D rb;
    public override Rigidbody2D Rigidbody => rb;
    public float seekRadius;
    public float wander;
    public float speed;
    public EntityBehaviour behaviour;
    public bool fireArrows;
    public Cooldown arrowCooldown;
    public Cooldown stopWhileFireing;
    public FireProjectile arrowSettings;
    [Tooltip("Time in between searches for the player")]
    public Cooldown seekTimer;
    public Sprite bulletSprite;
    public float deadZone;
    private Animator anim;
    [Tooltip("Time disabled after getting hit")]
    public Cooldown hitstun;
    public Vector2 Wander()
    {
        return UnityEngine.Random.insideUnitCircle * wander;
    }
    public Func<Vector2> getDir = null;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        var i = Wander();
        seeker = () => i;
        
        anim = GetComponent<Animator>();
    }
    public override bool DealDamage(int damage, float force, Vector2 from)
    {
        hitstun.Use();
       return base.DealDamage(damage, force, from);      
    }
    public override void Die()
    {
        //For archer while no sprite
        if (fireArrows)
        {
            base.Die();
            return;
        }

        //For skele warror
        if (hp <= 0)
        {
            anim.SetTrigger("Dead");
        }
        Neutralize();
        Invoke("Disappear", 6f);
    }
    //Make slime stop attacking, corpse sits there for a bit
    private void Neutralize()
    {
        hitstun.Use(10f);
        foreach (Collider2D col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }
    }
    private void Disappear()
    {
        IEnumerator fade = FadeAway(2f);
        StartCoroutine(fade);
    }
    Func<Vector2> seeker;
    Transform target = null;
    // Update is called once per frame
    void Update()
    {
        if (seekTimer.IsReady && target == null)
        {
            if ((Player.Instance.transform.position - this.transform.position).magnitude < seekRadius)
            {
                target = Player.Instance.transform;
                seeker = behaviour.GetBehaviour(target);
            }
            else
            {
                var i = Wander();
                seeker = () => i;
            }
        }

        //fireArrows
        if (target != null && fireArrows && arrowCooldown.IsReady)
        {
            var bul = arrowSettings.Execute(this.transform, target.position - this.transform.position);
            bul.GetComponent<SpriteRenderer>().sprite = bulletSprite;
            arrowCooldown.Use();
            stopWhileFireing.Use();
        }

        //Anim updates
        if(anim != null)
        {
            if(rb.velocity.sqrMagnitude > .05f)
            {
                anim.SetBool("isMoving", true);
                
            } else
            {
                anim.SetBool("isMoving", false);
            }
            if (target != null)
            {
                Vector2 dist = target.position - transform.position;
                float absDist = dist.magnitude;
                if (absDist < 1.8f)
                {
                    anim.SetTrigger("Attack");
                }
                anim.SetFloat("xInput", dist.normalized.x);
                anim.SetFloat("yInput", dist.normalized.y);
            } else
            {
                anim.SetFloat("xInput", rb.velocity.normalized.x);
                anim.SetFloat("yInput", rb.velocity.normalized.y);
            }
        }
    }
    private void FixedUpdate()
    {
        if (!stopWhileFireing.IsReady)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        //set movement
        Vector2 dir = seeker() - (Vector2)transform.position;
        if (dir.magnitude < deadZone)
        {
            dir = Vector2.zero;
        }

        //move away from nearby monsters
        var nearby = Physics2D.OverlapCircleAll(transform.position, 1);
        foreach (var other in nearby)
        {
            if (other.gameObject.CompareTag("Monster"))
            {
                Vector2 vec = (other.transform.position - transform.position);
                float dist2 = vec.sqrMagnitude;
                dist2 = 1 / (1 + dist2);
                dir += -vec.normalized * dist2;
            }
        }
        if (hitstun.IsReady)
        {
            rb.velocity = dir.normalized * speed;
        }

    }
}

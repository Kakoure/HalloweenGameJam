using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlimeController : Entity
{
    private Rigidbody2D rb;
    private Animator anim;
    public override Rigidbody2D Rigidbody => rb;
    public float chaseDistance;
    public float aggressiveDist;
    public float jumpStr;
    public float passiveStr;
    private float actionTime = 0;
    private Player target = null;


    #region Timer members

    private Timer jumper = new Timer();

    public float invalidAttemptCooldown;
    public Timer.TimedAction jumpAggressive;
    public Timer.TimedAction jumpWalk;

    private object jumpData;
    private Coroutine aiCoroutine;

    private void Wander()
    {
        Vector2 dir = UnityEngine.Random.insideUnitCircle;
        Jump((Vector2)transform.position + dir, passiveStr, false);
    }
    private void Jump(Vector2 location, float str, bool setVel)
    {

        Vector2 dir = location - (Vector2)transform.position;
        dir = dir.normalized;
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

        if (setVel) rb.velocity = dir.normalized * str; //override current velocity
        else rb.velocity += dir.normalized * str; //add to current velocity
    }
    private bool JumpAggressiveTest()
    {
        return target != null && (target.transform.position - transform.position).sqrMagnitude < aggressiveDist * aggressiveDist;
    }
    private void JumpAggressiveAction()
    {
        //Jump(target.transform.position, jumpStr, true);
        StartCoroutine("JumpAgressiveSequence");
    }
    private IEnumerator JumpAgressiveSequence()
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(.5f);
        Jump(target.transform.position, jumpStr, true);
    }
    private void JumpWalkAction()
    {
        if (target != null)
        {
            Jump(target.transform.position, passiveStr, false);
        }
        else
        {
            Wander();
        }
    }

    #endregion

    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        jumper.passiveWait = invalidAttemptCooldown;
        jumpAggressive.test = JumpAggressiveTest;
        jumpAggressive.action = JumpAggressiveAction;
        jumpWalk.test = () => true;
        jumpWalk.action = JumpWalkAction;
        jumper.AddAction(jumpAggressive);
        jumper.AddAction(jumpWalk);
        aiCoroutine = StartCoroutine(jumper.TimerCoroutine());
    }
    public override void Die()
    {
        if(hp <= 0)
        {
            anim.SetBool("Dead", true);
        }
        Neutralize();
        Invoke("Disappear", 6f);
    }
    //Make slime stop attacking, corpse sits there for a bit
    private void Neutralize()
    {
        StopCoroutine(aiCoroutine);
        foreach(Collider2D col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }
    }
    //Kill slime for good
    private void Disappear()
    {
        IEnumerator fade = FadeAway(2f);
        StartCoroutine(fade);
    }
    void FixedUpdate()
    {
        /*
        if (Time.time > actionTime)
        {
            Action();
            actionTime = Time.time + jumpCooldown;
        }
        */
        target = null;
        if ((Player.Instance.transform.position - transform.position).sqrMagnitude < chaseDistance * chaseDistance) target = Player.Instance;
    }
}

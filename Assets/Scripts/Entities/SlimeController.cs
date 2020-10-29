using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlimeController : Entity
{
    private Rigidbody2D rb;
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


    private void Wander()
    {
        Vector2 dir = Random.insideUnitCircle;
        Jump((Vector2)transform.position + dir, passiveStr);
    }
    private void Jump(Vector2 location, float str)
    {
        var nearby = Physics2D.OverlapCircleAll(transform.position, 1);

        Vector2 dir = location - (Vector2)transform.position;
        dir = dir.normalized;
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
        rb.velocity = dir.normalized * str;
    }
    private bool JumpAggressiveTest()
    {
        return target != null && (target.transform.position - transform.position).sqrMagnitude < aggressiveDist * aggressiveDist;
    }
    private void JumpAggressiveAction()
    {
        Jump(target.transform.position, jumpStr);
    }

    private void JumpWalkAction()
    {
        if (target != null)
        {
            Jump(target.transform.position, passiveStr);
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
        StartCoroutine(jumper.TimerCoroutine());
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

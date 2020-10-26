using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlimeController : Entity
{
    private Rigidbody2D rb;
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
        rb.velocity=(location - (Vector2)transform.position).normalized * str;
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

    //returns squrmagnitude
    private float getDist(Component ob)
    {
        if (ob != null)
            return ((Vector2)ob.transform.position - (Vector2)transform.position).sqrMagnitude;
        else
            return Mathf.Infinity;
    }
    private float getDist(GameObject ob)
    {
        if (ob != null)
            return ((Vector2)ob.transform.position - (Vector2)transform.position).sqrMagnitude;
        else
            return Mathf.Infinity;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
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

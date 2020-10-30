using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CooldownTimer;

public partial class PlayerMove : MonoBehaviour
{
    private const float deadZone = .1f;
    private static Player player;
    public Rigidbody2D rb;

    public float speed = 5;
    public float diveDur;
    public float diveCoef;
    public Cooldown jumpCooldown;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = Player.Instance;
    }

    // Update is called once per frame
    float xAxis;
    float yAxis;
    float xAxisRaw;
    float yAxisRaw;
    bool dodge;
    Vector2 facingDir;
    void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        dodge = Input.GetButtonDown("Jump");

        if (dodge && jumpCooldown.IsReady)
        {
            //Check Raw inputs, keyboard only
            xAxisRaw = Input.GetAxisRaw("Horizontal");
            yAxisRaw = Input.GetAxisRaw("Vertical");
            //Store last control input for anim facing dir
            if (xAxisRaw != 0 || yAxisRaw != 0)
            {
                anim.SetFloat("xInput", xAxisRaw);
                anim.SetFloat("yInput", yAxisRaw);
                facingDir.Set(xAxisRaw, yAxisRaw);
            }

            SetPath(Boomerang.Mult(speed * diveCoef, RollPath(facingDir.normalized)), diveDur);
            player.damageInvuln.Use();
            jumpCooldown.Use();

            //Anim parameter updates
            anim.SetTrigger("Dodge");
           
        }
    }

    private void FixedUpdate()
    {
        if (PathEnd)
        {
            //Check Raw inputs, keyboard only
            xAxisRaw = Input.GetAxisRaw("Horizontal");
            yAxisRaw = Input.GetAxisRaw("Vertical");
            //Store last control input for anim facing dir
            if (xAxisRaw != 0 || yAxisRaw != 0)
            {
                anim.SetFloat("xInput", xAxisRaw);
                anim.SetFloat("yInput", yAxisRaw);
                facingDir.Set(xAxisRaw, yAxisRaw);
            }

            //Set moving state for animator
            if (Mathf.Abs(xAxis) >= deadZone || Mathf.Abs(yAxis) >= deadZone)
            {
                anim.SetBool("isMoving", true);
            } else
            {
                anim.SetBool("isMoving", false);
            }

            rb.velocity = Vector2.ClampMagnitude(new Vector2(xAxis, yAxis), 1) * speed;
        }
        else
        {
            //velocity is determined purely by the path if I am in a path
            Vector2 v = GetPathVel();
            rb.velocity = v;
        }
    }
}

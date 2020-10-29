﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CooldownTimer;

public partial class PlayerMove : MonoBehaviour
{
    private static Player player;
    public Rigidbody2D rb;

    public float speed = 5;
    public float diveDur;
    public float diveCoef;
    public Cooldown jumpCooldown;

    [NonSerialized]
    [Obsolete]
    public Vector2 outsideForce;
    [Obsolete]
    private float end;

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
    void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        dodge = Input.GetButtonDown("Jump");

        

        if (dodge && jumpCooldown.IsReady)
        {
            Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            SetPath(Boomerang.Mult(speed * diveCoef, RollPath(dir)), diveDur);
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
            }

            rb.velocity = new Vector2(xAxis, yAxis) * speed;
            //Set moving state for animator
            if (Mathf.Abs(xAxis) >= .1f || Mathf.Abs(yAxis) >= .1f)
            {
                anim.SetBool("isMoving", true);
            } else
            {
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            //velocity is determined purely by the path if I am in a path
            Vector2 v = GetPathVel();
            rb.velocity = v;
        }
    }
}

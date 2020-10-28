using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CooldownTimer;

public partial class PlayerMove : MonoBehaviour
{
    private static Player player;
    public Rigidbody2D rb;

    public float speed;
    public float diveDur;
    public float diveCoef;
    public Cooldown jumpCooldown;

    [NonSerialized]
    [Obsolete]
    public Vector2 outsideForce;
    [Obsolete]
    private float end;

    private void Start()
    {
        player = Player.Instance;
    }

    // Update is called once per frame
    float xAxis;
    float yAxis;
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
        }
    }

    private void FixedUpdate()
    {
        if (PathEnd)
        {
            rb.velocity = new Vector2(xAxis, yAxis) * speed;
        }
        else
        {
            //velocity is determined purely by the path if I am in a path
            Vector2 v = GetPathVel();
            rb.velocity = v;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D rb;

    public float speed;

    [NonSerialized]
    public Vector2 outsideForce;
    private float end;

    public void SetOutsideForce(Vector2 outsideForce, float dur)
    {
        this.outsideForce = outsideForce;
        end = Time.time + dur;
    }

    Boomerang.Path GetPath(Vector2 facingDirection) => f => facingDirection * c(f);
    Boomerang.Converter jumpSpd = f => f < 0.6f ? 4.0f / 3 : 1.0f / 2;
    Boomerang.Converter c = f => f < 0.6f ? 4 * f / 3 : f / 2 + 0.5f;

    private void Start()
    {

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
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xAxis, yAxis) * speed;
        rb.velocity += outsideForce;
        if (Time.time > end)
        {
            end = Mathf.Infinity;
            outsideForce = Vector2.zero;
        }
    }
}

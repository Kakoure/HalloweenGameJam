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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(xAxis, yAxis) * speed;
        rb.velocity += outsideForce;
        if (Time.time > end)
        {
            end = Mathf.Infinity;
            outsideForce = Vector2.zero;
        }
    }
}

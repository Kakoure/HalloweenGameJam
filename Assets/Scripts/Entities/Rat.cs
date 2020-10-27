using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Entity
{
    private Rigidbody2D rb;
    public override Rigidbody2D Rigidbody => rb;
    public float seekRadius;
    public float wander;
    public float walkspeed;
    public float runspeed;
    public float runTime;

    public Vector2 Wander()
    {
        return UnityEngine.Random.insideUnitCircle * wander;
    }
    public Func<Vector2> getDir = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

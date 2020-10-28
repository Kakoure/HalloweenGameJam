﻿using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    public int damage;
    public float force;

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Collide with player");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collide with player");
            Entity entity = collision.GetComponent<Entity>();
            bool success = entity.DealDamage(damage, force, transform.position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    static float forceDuration = 0.5F;

    public int damage;
    public float force;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Dealt {damage} damage");
            if (force != 0)
            {
                Vector2 force = (collision.transform.position - transform.position).normalized * this.force;
                collision.transform.GetComponent<PlayerMove>().SetOutsideForce(force, forceDuration);
            }
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

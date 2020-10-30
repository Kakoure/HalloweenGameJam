using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    int Damage { get; }
    float Speed { get; }

    void Initialize(int damage, float vel);
}

//This has to be abstract because there are some monobehaviour methods that i cant do anything about
public abstract class Projectile : MonoBehaviour, IProjectile
{
    public int Damage { get; protected set; }
    public float Speed { get => RB.velocity.magnitude; }
    public Vector2 Velocity
    {
        get
        {
            return RB.velocity;
        }
        set
        {
            RB.velocity = value;
        }
    }

    private Rigidbody2D rb;
    public Rigidbody2D RB
    {
        get
        {
            if (rb == null)
            {
                rb = gameObject.GetComponent<Rigidbody2D>();
            }
            return rb;
        }
    }

    public virtual void Initialize(int damage, float vel)
    {
        Debug.Log("Initialized with " + damage);
        this.Damage = damage;
        this.RB.velocity = transform.right * vel;
    }
}
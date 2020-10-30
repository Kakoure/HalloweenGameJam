using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Boomerang;

public class BoomerangProj : MonoBehaviour, IProjectile
{
    [NonSerialized]
    public Boomerang boomer;

    public float dur;
    public int damage;
    public int Damage { get; set; }
    public float Speed => 0;
    private Vector2 src;

    public void Awake()
    {
        boomer = GetComponent<Boomerang>();
        gameObject.SetActive(false);
        Damage = damage;
    }

    private void Update()
    {
        transform.position = boomer.GetCurrent(Time.time) + src;
        if (Time.time > boomer.End) Die();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            // collide and die
            Player p = collision.GetComponent<Player>();
            bool success = p.DealDamage(Damage, 0, transform.position); // no force yet
            if (success) this.Die();
        }
    }

    public void Initialize(int damage, float vel = 0)
    {
        this.Damage = damage;
    }
    public void Die() => gameObject.SetActive(false);
   
    public void Fire(Vector2 src)
    {
        this.src = src;
        gameObject.SetActive(true);
        boomer.startTime = Time.time;
    }
    public void Fire(Vector2 src, Path p, float dur)
    {
        this.boomer.path = p;
        this.boomer.dur = dur;
        this.Fire(src);
    }
}


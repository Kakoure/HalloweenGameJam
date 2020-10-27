using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BoomerangProj : MonoBehaviour, IProjectile
{
    [NonSerialized]
    public Boomerang boomer;

    public float dur;
    public int Damage { get; set; }
    public float Speed => 0;
    private Vector2 src;

    public void Awake()
    {
        boomer = GetComponent<Boomerang>();
        boomer.startTime = Time.time;
        boomer.dur = dur;
    }

    private void Update()
    {
        transform.position = boomer.getCurrent(Time.time) + src;
        if (Time.time > boomer.End) KillObject();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            // collide and die
            Player p = collision.GetComponent<Player>();
            p.DealDamage(Damage);
        }

    }

    public void Initialize(int damage, float vel = 0)
    {
        this.Damage = damage;
    }
    public void KillObject() => gameObject.SetActive(false);

    public void Fire(Vector2 src)
    {
        this.src = src;
        gameObject.SetActive(true);
    }
}


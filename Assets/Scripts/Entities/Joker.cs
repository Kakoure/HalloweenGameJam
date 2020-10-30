using CooldownTimer;
using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Boomerang;

public partial class Joker : Entity
{
    public BoomerangProj p1;
    private Boomerang b1;
    public BoomerangProj p2;
    private Boomerang b2;

    public Transform testTarget;
    public float boomerangRadius;
    public float boomerangTime;
    public float shotSpread;
    public int shotDamage;
    public int shotKnockback = 1;
    public int shotSpeed;
    public Sprite shotSprite;
    public Cooldown boomerangTimer;
    public Cooldown altTimer;
    public Cooldown mainTimer;
    public int teleportHP1;
    public Transform secondTeleport;
    public int teleportHP2;
    public Transform thirdTeleport;

    FireProjectile fire;

    public override void Awake()
    {
        base.Awake();
        //_rb = GetComponent<Rigidbody2D>();
    }
    private int teleportHP;
    private Transform teleport;
    public override void Start()
    {
        base.Start();

        b1 = p1.GetComponent<Boomerang>();
        b2 = p2.GetComponent<Boomerang>();

        path1 = Polar180(Poly(6));
        path1 = Mult(boomerangRadius, path1);
        path2 = Negx(path1);

        p1.Fire(this.transform.position, path1, boomerangTime);
        p2.Fire(this.transform.position, path2, boomerangTime);

        mainCycle = MainCycler;
        altCycle = FireAtTarget;

        teleportHP = teleportHP1;
        teleport = secondTeleport;

        fire = new FireProjectile(CameraReference.Instance.bulletGeneric, shotDamage, shotKnockback, shotSpeed);
    }

    public void FireBoomerangs(Path path1, Path path2, float time = -1)
    {
        if (!boomerangTimer.IsReady) return;
        if (time < 0) time = boomerangTime; //use boomerangTime as default
        p1.Fire(this.transform.position, path1, time);
        p2.Fire(this.transform.position, path2, time);
        boomerangTimer.Use();
    }
    public void FireAt(Vector2 location)
    {
        Vector2 dir = location - (Vector2)this.transform.position;
        dir = dir.normalized;
        Vector2 dir2 = Quaternion.Euler(0, 0, shotSpread) * dir;
        Vector2 dir3 = Quaternion.Euler(0, 0, -shotSpread) * dir;

        fire.Execute(transform, dir).GetComponent<SpriteRenderer>().sprite = shotSprite;
        fire.Execute(transform, dir2).GetComponent<SpriteRenderer>().sprite = shotSprite;
        fire.Execute(transform, dir3).GetComponent<SpriteRenderer>().sprite = shotSprite;
    }

    //amazing recursion
    //some really interesting feature of C# is that you can declare a delegate type that depends on itself
    public delegate float Cycle(ref Cycle c);
    private Cycle mainCycle;
    private Cycle action;// the current state called every timer cooldown
    private Cycle altCycle;

    public override Rigidbody2D Rigidbody => null;

    public void Update()
    {
        if (hp <= teleportHP)
        {
            action = null;
            altCycle = null;
            mainCycle = TPAndStartMain(teleport);

            //dumb code
            teleportHP = teleportHP2;
            teleportHP2 = -1;
            teleport = thirdTeleport;
        }

        if (mainTimer.IsReady)
        {
            mainCycle(ref mainCycle);
            mainTimer.Use();
        }
        if (action != null && boomerangTimer.IsReady)
        {
            action(ref action);
            boomerangTimer.Use();
        }
        if (altCycle != null && altTimer.IsReady)
        {
            altCycle(ref altCycle);
            altTimer.Use();
        }
    }

    protected override void ApplyImpulse(float force, Vector2 from)
    {
        //no impulse for you
        return;
    }
}


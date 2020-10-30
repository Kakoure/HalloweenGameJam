using CooldownTimer;
using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.iOS;
using static Boomerang;

public partial class Joker : Entity
{
    public BoomerangProj p1;
    private Boomerang b1;
    public BoomerangProj p2;
    private Boomerang b2;
    public BoomerangProj p3;
    public BoomerangProj p4;

    public Transform testTarget;
    public float boomerangRadius;
    public float boomerangTime;
    public float shotSpread;
    public int shotDamage;
    public int shotKnockback = 1;
    public int shotSpeed;
    //public Sprite shotSprite;
    public GameObject shotPrefab;
    public GameObject ballPrefab;
    public GameObject slimePrefab;

    //cooldowns are basically clocks
    public Cooldown boomerangTimer;
    public Cooldown altTimer;
    public Cooldown mainTimer;

    public int teleportHP1;
    public Transform firstTeleport;
    public int teleportHP2;
    public Transform secondTeleport;
    public int teleportHP3;
    public Transform thirdTeleport;


    public float teleportFromTime = 1;
    public float teleportToTime = 1;
    public float defaultAttackTime = 1;
    public float fireTime = 1;
    public float windupTime = 1;
    public float phaseTime = 8;

    FireProjectile fire;

    public bool IsInPhaseTwo => hp <= 500;

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

        //mainCycle = TPAndStart(this.transform, MainCycler);

        teleportHP = teleportHP1;
        teleport = firstTeleport;

        currentAttackTime = defaultAttackTime;

        fire = new FireProjectile(shotPrefab, shotDamage, shotKnockback, shotSpeed);
        fireBall = new FireProjectile(ballPrefab, 10, 1, 5, 2);

        boomerangCycle = Burst6;

        invulnCooldown = new Cooldown(teleportFromTime + teleportToTime);
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

        Bullet bullet = fire.Execute(transform, dir);
        Bullet bullet1 = fire.Execute(transform, dir2);
        Bullet bullet2 = fire.Execute(transform, dir3);
        
    }
    //warning: this overload might produce unexpected behaviour
    public void FireAt(Vector2 dir, uint numLeft, float spread)
    {
        dir = dir.normalized;
        Bullet bullet = fire.Execute(transform, dir);
        for (int i = 0; i < numLeft; i++)
        {
            dir = Quaternion.Euler(0, 0, spread) * dir;
            _ = fire.Execute(transform, dir);
        }
    }

    //amazing recursion
    //some really interesting feature of C# is that you can declare a delegate type that depends on itself
    public delegate float Cycle(ref Cycle c);
    private Cycle mainCycle;
    private Cycle boomerangCycle;// the current state called every timer cooldown
    private Cycle altCycle;

    public override Rigidbody2D Rigidbody => null;

    private float currentAttackTime;
    public void Update()
    {
        if (hp <= teleportHP)
        {
            Cycle c;

            if (hp > teleportHP2) c = Burst6;
            else c = Burst8(0);

            boomerangCycle = TPFrom(teleport.position, c);
            Debug.Log(teleport);

            if (teleportHP == teleportHP3) { teleportHP = -1; }
            if (teleportHP == teleportHP2) { teleportHP = teleportHP3; teleport = thirdTeleport; }
            if (teleportHP == teleportHP1) { teleportHP = teleportHP2; teleport = secondTeleport; }
            boomerangTimer.Use(0);
            invulnCooldown.Use();
        }

        //three timers being maintained
        //cycles are found in JokerCycles.cs
        if (mainCycle != null && mainTimer.IsReady)
        {
            float t = mainCycle(ref mainCycle);
            if (t >= 0)
                //uses return value time
                mainTimer.Use(t);
            else
                //uses previous time
                mainTimer.Use();
        }
        if (boomerangCycle != null && boomerangTimer.IsReady)
        {
            float t = boomerangCycle(ref boomerangCycle);
            if (t >= 0)
                boomerangTimer.Use(t);
            else
                boomerangTimer.Use(defaultAttackTime);
        }
        if (altCycle != null && altTimer.IsReady)
        {
            float t = altCycle(ref altCycle);
            if (t >= 0)
                altTimer.Use(t);
            else
                altTimer.Use(currentAttackTime);
        }
    }

    private Cooldown invulnCooldown; //start
    protected override void ApplyImpulse(float force, Vector2 from)
    {
        //no impulse for you
        return;
    }
    public override bool DealDamage(int damage, float force, Vector2 from)
    {
        if (!invulnCooldown.IsReady)
            return false;
        else
            return base.DealDamage(damage, force, from);
    }
    public override void Die()
    {
        //death Animation



        Destroy(this);
    }
}


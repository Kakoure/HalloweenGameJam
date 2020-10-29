using CooldownTimer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Boomerang;

public class Joker : MonoBehaviour
{
    public BoomerangProj p1;
    private Boomerang b1;
    public BoomerangProj p2;
    private Boomerang b2;

    public float boomerangRadius;
    public float boomerangTime;
    public Cooldown boomerangTimer;

    public Path path1;
    public Path path2;
    public Path Path3 => Negy(path1);
    public Path Path4 => Negy(path2);
    public Path Path5 => Flipxy(path1);
    public Path Path6 => Negy(Path5);
    public Path Path7 => Negx(Path5);
    public Path Path8 => Negx(Path6);

    private void Start()
    {
        b1 = p1.GetComponent<Boomerang>();
        b2 = p2.GetComponent<Boomerang>();

        path1 = Polar180(Poly(6));
        path1 = Mult(boomerangRadius, path1);
        path2 = Negx(path1);

        p1.Fire(this.transform.position, path1, boomerangTime);
        p2.Fire(this.transform.position, path2, boomerangTime);

        action = Phase1;
    }

    public void FireBoomerangs(Path path1, Path path2)
    {
        if (!boomerangTimer.IsReady) return;
        p1.Fire(this.transform.position, path1, boomerangTime);
        p2.Fire(this.transform.position, path2, boomerangTime);
        boomerangTimer.Use();
    }

    private Action action;// the current state called every update
    
    void Phase1()
    {
        if (!boomerangTimer.IsReady) return;

        //do somthing
        FireBoomerangs(path1, path2);

        boomerangTimer.Use();
        action = Phase2;
    }
    void Phase2()
    {
        if (!boomerangTimer.IsReady) return;

        //do somthing
        FireBoomerangs(Path3, Path4);

        boomerangTimer.Use();
        action = Phase3;
    }
    void Phase3()
    {
        if (!boomerangTimer.IsReady) return;

        //do somthing
        FireBoomerangs(Path5, Path6);

        boomerangTimer.Use();
        action = Phase4;
    }
    void Phase4()
    {
        if (!boomerangTimer.IsReady) return;

        //do somthing
        FireBoomerangs(Path7, Path8);

        boomerangTimer.Use();
        action = Phase1;
    }
    public void Update()
    {
        action();
    }
    
}


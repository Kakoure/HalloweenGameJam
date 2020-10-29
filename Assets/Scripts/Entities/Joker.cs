using CooldownTimer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Boomerang;

public partial class Joker : MonoBehaviour
{
    public BoomerangProj p1;
    private Boomerang b1;
    public BoomerangProj p2;
    private Boomerang b2;

    public Transform testTarget;
    public float boomerangRadius;
    public float boomerangTime;
    public Cooldown boomerangTimer;
    public Cooldown altTimer;
    public Cooldown mainTimer;


    private void Start()
    {
        b1 = p1.GetComponent<Boomerang>();
        b2 = p2.GetComponent<Boomerang>();

        path1 = Polar180(Poly(6));
        path1 = Mult(boomerangRadius, path1);
        path2 = Negx(path1);

        p1.Fire(this.transform.position, path1, boomerangTime);
        p2.Fire(this.transform.position, path2, boomerangTime);

        mainCycle = MainCycler;
    }

    public void FireBoomerangs(Path path1, Path path2, float time = -1)
    {
        if (!boomerangTimer.IsReady) return;
        if (time < 0) time = boomerangTime; //use boomerangTime as default
        p1.Fire(this.transform.position, path1, time);
        p2.Fire(this.transform.position, path2, time);
        boomerangTimer.Use();
    }

    //amazing recursion
    //some really interesting feature of C# is that you can declare a delegate type that depends on itself
    public delegate float Cycle(ref Cycle c);
    private Cycle mainCycle;
    private Cycle action;// the current state called every update
    private Cycle altCycle;
    
    float MainCycler(ref Cycle self)
    {
        action = Phase1;
        self = Cycle2;
        return -1;
    }
    float Cycle2(ref Cycle self)
    {
        action = BoomerangCycle;
        self = MainCycler;
        return -1;
    }

    float Phase1(ref Cycle action)
    {
        //do somthing
        FireBoomerangs(path1, path2);

        action = Phase2;
        return -1;
    }
    float Phase2(ref Cycle action)
    {
        //do somthing
        FireBoomerangs(Path3, Path4);

        action = Phase3;
        return -1;
    }
    float Phase3(ref Cycle action)
    {
        //do somthing
        FireBoomerangs(Path5, Path6);

        action = Phase4;
        return -1;
    }
    float Phase4(ref Cycle action)
    {
        //do somthing
        FireBoomerangs(Path7, Path8);

        action = Phase1;
        return 1.5f;
    }

    float BoomerangCycle(ref Cycle action)
    {
        FireBoomerangs(wiggle, wiggle2);

        action = Boomer2;
        return -1;
    }
    float Boomer2(ref Cycle action)
    {
        FireBoomerangs(wiggle3, wiggle4);

        action = Boomer3;
        return -1;
    }
    float Boomer3(ref Cycle action)
    {
        FireBoomerangs(wiggle5, wiggle6);

        action = Boomer4;
        return -1;
    }
    float Boomer4(ref Cycle action)
    {
        FireBoomerangs(wiggle7, wiggle8);

        action = BoomerangCycle;
        return -1;
    }
    public void Update()
    {
        if (mainTimer.IsReady)
        {
            mainCycle(ref mainCycle);
            mainTimer.Use();
        }
        if (boomerangTimer.IsReady)
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
    
}


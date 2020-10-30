using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using UnityEngine;
using static Boomerang;

public partial class Joker
{
    public float fiveSecondIntervals = 5;
    public float juggleTime1 = 1.5f;

    #region Paths

    public Path path1; //assigned in starts
    public Path path2; //start
    public Path Path3 => Negy(path1);
    public Path Path4 => Negy(path2);
    public Path Path5 => Flipxy(path1);
    public Path Path6 => Negy(Path5);
    public Path Path7 => Negx(Path5);
    public Path Path8 => Negx(Path6);

    private const float magnitudeScalar = 2;
    public Path wiggle = Add(LinePath(SinC(2), new Vector2(-1, 1) * magnitudeScalar), LinePath(f => 7 * f, new Vector2(1, 1)));
    public Path wiggle2 => Flipxy(wiggle);
    public Path wiggle3 => Negy(wiggle5);
    public Path wiggle4 => Negy(wiggle6);
    public Path wiggle5 => Negx(wiggle);
    public Path wiggle6 => Negx(wiggle2);
    public Path wiggle7 => Negy(wiggle);
    public Path wiggle8 => Negy(wiggle2);

    #endregion

    #region Cycles

    Cycle TPFrom(Vector2 location, Cycle main) => (ref Cycle self) =>
    {
        //tpFromAnimationStart
        self = TPAndStart(location, main);

        return teleportFromTime;
    };
    Cycle TPAndStart(Vector2 location, Cycle main) => (ref Cycle self) =>
    {
        transform.position = location;

        boomerangTimer.Use();
        altTimer.Use();
        //mainTimer.Use();

        self = main;

        //tpTo animation start here


        return teleportToTime;
    };

    //everything below this is kinda irrelevent

    Cycle AttackPrep(float time, Cycle next) => (ref Cycle self) =>
    {
        //do attack prep here


        self = next;

        return time;
    };

    //fires while fireing Boomerangs
    float MainCycler2(ref Cycle self)
    {
        boomerangCycle = Phase1;
        altCycle = FireAtTarget;

        self = CycleShooting(MainCycler);

        return phaseTime;
    }
    float MainCycler(ref Cycle self)
    {
        boomerangCycle = Phase1;
        altCycle = Burst6;

        self = CycleShooting(MainCycler);
        
        return phaseTime;
    }
    Cycle Cycle2(Cycle mainLoopback) => (ref Cycle self) =>
    {
        boomerangCycle = BoomerangCycle;

        //loop back to main
        self = mainLoopback;

        altCycle = null;
        return phaseTime;
    };
    Cycle CycleShooting(Cycle mainLoopback) => (ref Cycle self) =>
    {
        boomerangCycle = FireAtTarget;
        self = Cycle2(mainLoopback);
        return phaseTime;
    };

    //for Boomerangs
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

        return -1;
    }

    //more Boomerangs
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

    //loop for fireing
    float FireAtTarget(ref Cycle action)
    {
        this.FireAt(testTarget.position);

        action = AttackPrep(windupTime, FireAtTarget);

        return fireTime;
    }

    #endregion

    const float defaultPrepDuration = .5f;

    Cycle PrepareAttack(Cycle next, float duration = defaultPrepDuration) => (ref Cycle action) =>
    {
        //put prepare code here

        action = next;

        return duration;
    };

    //entry point
    float Burst6(ref Cycle action)
    {
        FireAt(Vector2.up, 5, 60);

        //next cycler;
        action = PrepareAttack(Fire6(0));

        return fiveSecondIntervals;
    }

    private float TimeInBetweenShots => fiveSecondIntervals / 8;
    private float TimeAfterShots => fiveSecondIntervals - 6 * TimeInBetweenShots;
    Cycle Fire6(int counter) => (ref Cycle action) =>
    {
        fire.Execute(transform, testTarget.position - transform.position);

        if (++counter < 6)
        {
            //not actually recursion btw
            action = PrepareAttack(Fire6(counter));

            return TimeInBetweenShots;
        }
        else
        {
            //next cycler
            //dont prepare juggling
            action = PrepareAttack(Juggle1(0, 0));

            return TimeAfterShots;
        }
    };
    
    /*
    float BeginJuggle(ref Cycle action)
    {
        action = EndJuggle;

        //begin juggle
        altCycle = Phase1;//wierd name i know

        return 2 * fiveSecondIntervals;
    }
    float EndJuggle(ref Cycle action)
    {
        action = null; //next Cycle

        altCycle = null; // stop juggle

        return 0; //jank
    }
    */

    private float circleTime => fiveSecondIntervals / 6;
    private float timeAFterCircles => fiveSecondIntervals - circleTime * 3;
    private Converter RadialFunc =>  Mult(boomerangRadius, SinC(1)); //flower
    Cycle Juggle1(float del, int counter) => (ref Cycle action) =>
    {
        p1.Fire(this.transform.position, Polar(RadialFunc, 0 + del, 2 * Mathf.PI /3), juggleTime1);
        p2.Fire(this.transform.position, Polar(RadialFunc, 2 * Mathf.PI / 3 + del, 2 * Mathf.PI / 3), juggleTime1);
        p3.Fire(this.transform.position, Polar(RadialFunc, 4 * Mathf.PI / 3 + del, 2 * Mathf.PI / 3), juggleTime1);

        if (++counter < 4)
            action = Juggle1(del + Mathf.PI / 6, counter);
        else
            action = PrepareAttack(Burst6); //next Cycle


        return juggleTime1;
    };

    //phase2
    //second entry point
    Cycle Burst8(int counter) => (ref Cycle action) =>
    {
        FireAt(Vector2.up, 7, 45);

        if (++counter < 2)
            action = PrepareAttack(Burst8(counter));
        else
            action = PrepareAttack(Fire18(0));// next cycle

        return fiveSecondIntervals / 2;
    };

    private float timeBetweenShots18 => (fiveSecondIntervals - timeAfterBurst18) / 18;
    private float timeAfterBurst18 => 1;
    Cycle Fire18(int counter) => (ref Cycle action) =>
   {
       fire.Execute(transform, testTarget.position - transform.position);

       if (++counter < 18)
       {
           //not actually recursion btw
           action = PrepareAttack(Fire18(counter));

           return timeBetweenShots18;
       }
       else
       {
           //next cycler
           action = PrepareAttack(ThrowBall);

           return timeAfterBurst18;
       }
   };

    private FireProjectile fireBall; // start
    float ThrowBall(ref Cycle action)
    {
        var m = fireBall.Execute(this.transform, testTarget.position - this.transform.position);
        //summon two slimes on collision
        m.onCollision = () =>
        {
            Instantiate(slimePrefab, m.transform.position, Quaternion.identity);
            Instantiate(slimePrefab, m.transform.position, Quaternion.identity);
        };

        action = Juggle2(0, 0); //next cycle

        return fiveSecondIntervals;
    }

    float juggleTime2 = 1;
    Cycle Juggle2(float del, int counter) => (ref Cycle action) =>
    {
        p1.Fire(this.transform.position, Polar(RadialFunc, 0 + del, Mathf.PI / 2), juggleTime2);
        p2.Fire(this.transform.position, Polar(RadialFunc, Mathf.PI / 2 + del, Mathf.PI / 2), juggleTime2);
        p3.Fire(this.transform.position, Polar(RadialFunc, Mathf.PI + del, Mathf.PI / 2), juggleTime2);
        p4.Fire(this.transform.position, Polar(RadialFunc, 3 * Mathf.PI / 2 + del, Mathf.PI / 2), juggleTime2);

        if (++counter < 6)
            action = Juggle2(del + Mathf.PI / 6, counter);
        else
        {
            if (hp < teleportHP3)
            {
                //go to teleport
                Debug.Log("Teleporting");
                TPFrom(testTarget.transform.position, PrepareAttack(Burst8(0)));
            }
            else
                action = PrepareAttack(Burst8(0)); //next Cycle
        }

        return juggleTime1;
    };
}


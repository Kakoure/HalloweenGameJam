using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Boomerang;

public partial class Joker
{
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


    Cycle TPAndStartMain(Transform location) => (ref Cycle self) =>
    {
        transform.position = location.position;

        boomerangTimer.Use();
        altTimer.Use();
        mainTimer.Use();

        self = MainCycler;
        action = Phase1;
        altCycle = FireAtTarget;

        return -1;
    };
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

    float FireAtTarget(ref Cycle _)
    {
        this.FireAt(testTarget.transform.position - this.transform.position);

        return -1;
    }

    #endregion
}


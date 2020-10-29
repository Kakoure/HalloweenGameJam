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

    private void Start()
    {
        b1 = p1.GetComponent<Boomerang>();
        b2 = p2.GetComponent<Boomerang>();

        path1 = Polar180(Poly(6));
        path1 = Mult(boomerangRadius, path1);
        path2 = Negy(path1);

        p1.Fire(this.transform.position, path1, boomerangTime);
        p2.Fire(this.transform.position, path2, boomerangTime);
    }

    public void FireBoomerangs()
    {
        if (!boomerangTimer.IsReady) return;
        p1.Fire(this.transform.position, path1, boomerangTime);
        p2.Fire(this.transform.position, path2, boomerangTime);
        boomerangTimer.Use();
    }
}


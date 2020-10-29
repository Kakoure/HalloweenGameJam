using CooldownTimer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Joker : MonoBehaviour
{
    [NonSerialized]
    public BoomerangProj p1;
    private Boomerang b1;
    [NonSerialized]
    public BoomerangProj p2;
    private Boomerang b2;

    public Cooldown boomerangTimer;

    private void Start()
    {
        b1 = p1.GetComponent<Boomerang>();
        b2 = p2.GetComponent<Boomerang>();
    }
}


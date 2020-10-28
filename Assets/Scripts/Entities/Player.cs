using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Player : Entity
{
    public static Player Instance { get; private set; }

    private Rigidbody2D _rb;
    public override Rigidbody2D Rigidbody => _rb;

    public override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        if (Instance != null) Debug.LogError("Multiple players detected");
        Instance = this;
    }

    private void Start()
    {

    }

    int exp;
}

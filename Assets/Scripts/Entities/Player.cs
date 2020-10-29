using CooldownTimer;
using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

class Player : Entity
{
    public static Player Instance { get; private set; }

    private Rigidbody2D _rb;
    public override Rigidbody2D Rigidbody => _rb;
    [NonSerialized]
    public PlayerMove pm;

    public float damageKnockbackDur = 1;
    public float damageKnockbackCoef = 0;
    [Tooltip("Ammount of time player spends invulurable after taking damage")]
    public Cooldown damageInvuln;
    [Tooltip("Ammount of time player spends invulurable in dodge state")]
    public Cooldown dodgeInvuln;

    public bool IsInvuln => !dodgeInvuln.IsReady || !damageInvuln.IsReady;

    //deal damage and apply impulse can be merged
    //also apply impuls for playe ignores force parameter
    protected override void ApplyImpulse(float force, Vector2 from)
    {
        Vector2 v = (Vector2)transform.position - from;
        Debug.Log(damageKnockbackDur);
        pm.SetPath(PlayerMove.RollPath(damageKnockbackCoef * v.normalized), damageKnockbackDur);
    }
    public override bool DealDamage(int damage, float force, Vector2 from)
    {
        if (IsInvuln) return false;

        //updates the healthbar
        HP -= damage;
        healthBar.SetHealth(HP, MaxHP);
        CameraReference.Instance.InstantiateHitMarker(damage, transform.position);
        if (HP <= 0) Die();
        damageInvuln.Use();
        StartCoroutine("DamageFlash");
        ApplyImpulse(force, from);

        return true;
    }
    public override void Die()
    {
        SceneManager.LoadScene("RIP");
    }

    public override void Awake()
    {
        //base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        if (Instance != null) Debug.LogError("Multiple players detected");
        Instance = this;
        pm = GetComponent<PlayerMove>();
    }

    public override void Start()
    {
        base.Start();
    }

    int exp;
}

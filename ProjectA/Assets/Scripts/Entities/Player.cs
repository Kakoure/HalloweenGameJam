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
    public PlayerMove playerMove;
    [NonSerialized]
    public Animator playerAnimator;

    public float damageKnockbackDur = 1;
    public float damageKnockbackCoef = 0;
    [Tooltip("Ammount of time player spends invulurable after taking damage")]
    public Cooldown damageInvuln;
    [Tooltip("Ammount of time player spends invulurable in dodge state")]
    public Cooldown dodgeInvuln;
    public AudioClip attackSound;
    public AudioClip equipSound;

    public bool IsInvuln => !dodgeInvuln.IsReady || !damageInvuln.IsReady;

    //deal damage and apply impulse can be merged
    //also apply impuls for playe ignores force parameter
    protected override void ApplyImpulse(float force, Vector2 from)
    {
        if (force == 0) return; //if no force return

        Vector2 v = (Vector2)transform.position - from;
        playerMove.SetPath(PlayerMove.RollPath(damageKnockbackCoef * v.normalized), damageKnockbackDur);
    }
    public override bool DealDamage(int damage, float force, Vector2 from)
    {
        if (IsInvuln) return false;

        hp -= damage;
        hp = Mathf.Clamp(hp, 0, MaxHP); //make sure that the player is not overhealed
        if (damage > 0)
        {
            audioSrc.PlayOneShot(hurtSound);
        }
        //updates the healthbar
        healthBar.SetHealth(hp, MaxHP);
        CameraReference.Instance.InstantiateHitMarker(damage, transform.position);
        if (hp <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine("DamageFlash");
        }

        //activate damageInvulnurablilty
        damageInvuln.Use();

        //apply force
        ApplyImpulse(force, from);

        //if weapon implements IDamageTaken then call it
        IDamageTaken damageTaken = Inventory.CurrentWeapon as IDamageTaken;
        damageTaken?.OnDamageTaken(damage, from);

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
        playerMove = GetComponent<PlayerMove>();
        playerAnimator = GetComponent<Animator>();
    }

    public override void Start()
    {
        base.Start();
    }
    public void PlaySound(AudioClip sound)
    {
        audioSrc.PlayOneShot(sound);
    }
    public void PlayAttackSound()
    {
        audioSrc.PlayOneShot(attackSound);
    }
    int exp;
}

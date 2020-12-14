using CooldownTimer;
using Entities;
using Items;
using PlayerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    public static Player Instance { get; private set; }

    public Vector2 MousePositionRelative => CameraReference.Instance.camera.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;

    private Rigidbody2D _rb;
    public override Rigidbody2D Rigidbody => _rb;
    [NonSerialized]
    public PlayerMove playerMove;
    [NonSerialized]
    public Animator playerAnimator;

    public PlayerClass playerClass;
    public float damageKnockbackDuration = 1;
    public float damageKnockbackCoefficient = 0;
    [Tooltip("Ammount of time player spends invulurable after taking damage")]
    public Cooldown damageInvuln;
    [Tooltip("Ammount of time player spends invulurable in dodge state")]
    public Cooldown dodgeInvuln;

    //TODO: I can load this from resources instead of using a field
    public AudioClip attackSound;
    public AudioClip equipSound;

    public bool IsInvuln => !dodgeInvuln.IsReady || !damageInvuln.IsReady;

    //deal damage and apply impulse can be merged
    //also apply impulse for player ignores force parameter
    protected override void ApplyImpulse(float force, Vector2 from)
    {
        if (force == 0) return; //if no force return

        Vector2 v = (Vector2)transform.position - from;
        playerMove.SetPath(PlayerMove.RollPath(damageKnockbackCoefficient * v.normalized), damageKnockbackDuration);
    }
    public override bool DealDamage(int damage, float force, Vector2 from)
    {
        //replace this with an effect...
        if (IsInvuln) return false;

        //discrete Damage
        if (damage > 0)
        {
            //call base instead of doing all of that stuff
            base.DealDamage(1, force, from);
        }

        //activate damageInvulnurablilty
        damageInvuln.Use();

        //apply force
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
        if (Instance != null) UnityEngine.Debug.LogError("Multiple players detected");
        Instance = this;
        playerMove = GetComponent<PlayerMove>();
        playerAnimator = GetComponent<Animator>();
    }
    public override void Start()
    {
        base.Start();

        //assigns items to the class
        Inventory.Pickup(Inventory.GetSlot(Inventory.InventorySlot.SlotType.Weapon), Item.InstantiateItem(playerClass.Weapon));
        Inventory.Pickup(Inventory.GetSlot(Inventory.InventorySlot.SlotType.Shield), Item.InstantiateItem(playerClass.Offhand));

        foreach (var item in playerClass.Items)
        {
            int slot = Inventory.GetEmptyInventorySlot();
            if (slot != -1) Inventory.Pickup(Inventory.GetSlot(Inventory.InventorySlot.SlotType.Inventory, slot), Item.InstantiateItem(item));
        }
    }

    public void PlaySound(AudioClip sound)
    {
        audioSrc.PlayOneShot(sound);
    }
    public void PlayAttackSound()
    {
        audioSrc.PlayOneShot(attackSound);
    }

    //will eventually get to this
    int exp;
}

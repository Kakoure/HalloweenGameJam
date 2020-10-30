using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CooldownTimer;
using UnityEngine.UI;
using static Boomerang;
public class Sword : Weapon
{
    public static int swordMass = 10;

    public float radius;
    public float arcRadians; //sq
    public int damage;
    public float force;
    public float lungeForce;
    private Sprite _sprite;


    private Cooldown comboReset;
    //Initalized in start
    Converter lungeConverter = f => f < .1f ? 0f : Mathf.Max(0, (2 - Mathf.Pow((f + .5f), 2)));
    public override Sprite Sprite => _sprite;

    public override void AltFire(Transform player, bool down)
    {
        if (down)
        {
            Bullet bullet = throwitem.Execute(player, out _);
            bullet.GetComponent<SpriteRenderer>().sprite = Sprite;
            bullet.onCollision = () =>
            {
                //drop this item
                this.DropAt(bullet.transform.position);
            };
            Inventory.PopFromSlot(Inventory.Instance.shield);
        }
    }

    public override void Fire(Transform player, bool down)
    {
        if (!down) return;
        if (!IsReady) return;
        if (!Player.Instance.pm.PathEnd) return;
        StartCoroutine("AttackSequence");
        SetUseTime();
    }

    private FireProjectile throwitem;

    // Start is called before the first frame update
    void Start()
    {
        throwitem = FireProjectile.ThrowProjectile(Mass, 0.5f);
        _sprite = GetComponent<SpriteRenderer>().sprite;
        id = ItemID.Sword;
        lungeConverter = f => f < .3f ? 0f : lungeForce * Mathf.Max(0, (2 - Mathf.Pow((f + .3f), 2)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sword() : base(swordMass)
    {

    }
    private void HitScan()
    {
        Transform player = Player.Instance.gameObject.transform;
        var collisions = Physics2D.OverlapCircleAll(player.position, radius);
        foreach (var col in collisions)
        {
            if (col.gameObject.CompareTag("Monster"))
            {

                Vector2 disp = col.transform.position - player.position;
                Vector2 cDisp = CameraReference.Instance.camera.ScreenToWorldPoint(Input.mousePosition) - player.position;
                double arc = Mathf.Atan2(disp.x, disp.y) - Math.Atan2(cDisp.x, cDisp.y);
                if (arc * arc < arcRadians * arcRadians)
                {
                    //deal damage
                    Entity entity = col.GetComponent<Entity>();
                    bool success = entity.DealDamage(damage, force, player.position);
                }
            }
        }
    }
    private IEnumerator AttackSequence()
    {
        
        Vector2 lookDir = (CameraReference.Instance.camera.ScreenToWorldPoint(Input.mousePosition) - Player.Instance.gameObject.transform.position).normalized;
        
        Animator anim = Player.Instance.GetComponent<Animator>();
        anim.SetTrigger("Attack");
        if (comboReset.IsReady)
        {
            anim.SetInteger("Combo", 0);
        }else if(anim.GetInteger("Combo") == 0)
        {
            anim.SetInteger("Combo", 1);
        } else
        {
            anim.SetInteger("Combo", 0);
        }
        comboReset.Use(1f);
        anim.SetFloat("xInput", lookDir.x);
        anim.SetFloat("yInput", lookDir.y);
        
        Path lungePath = LinePath(lungeConverter, lookDir);
        Player.Instance.pm.SetPath(Boomerang.Mult(Player.Instance.pm.speed, lungePath), .5f);
        Player.Instance.damageInvuln.Use();


        yield return new WaitForSeconds(.2f);
        HitScan();
        yield return new WaitForSeconds(.2f);
        HitScan();
    }
}

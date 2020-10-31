using Items;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell : Weapon
{
    static readonly int SpellMass = 1;

    public int damage;
    public float force;
    public int explosionDamage;
    public float explosionForce;
    public float explosionRadius;
    public float spd;
    public GameObject fireball;

    private Sprite _sprite;
    public override Sprite Sprite => _sprite;

    public override void AltFire(Transform player, bool down)
    {
        Fire(player, down);
    }

    public override void Fire(Transform player, bool down)
    {
        if (down)
        {
            if (!IsReady) return;

            //fire a projectile
            Fireball bullet = (Fireball)fireProjectile.Execute(player, out Vector2 dir);
            bullet.fuse = Time.time + dir.magnitude / spd;
            bullet.explosionDamage = explosionDamage;
            bullet.explosionForce = explosionForce;
            bullet.explosionRadius = explosionRadius;
            Player.Instance.PlaySound(useSound);

            SetUseTime();
        }
    }

    private FireProjectile fireProjectile;
    private void Awake()
    {
        fireProjectile = new FireProjectile(fireball, damage, 0, spd);
    }

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>().sprite;
        id = ItemID.Staff;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Spell() : base(SpellMass)
    {

    }
}

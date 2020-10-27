using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell : Item
{
    static int swordMass = 1;

    public int damage;
    public float force;
    public float fireTime;
    public int explosionDamage;
    public float explosionForce;
    public float explosionRadius;
    public float spd;
    public GameObject fireball;

    private Sprite _sprite;
    private float nextFire = 0;
    public override Sprite Sprite => _sprite;

    public override void AltFire(Transform player)
    {
        Fire(player);
    }

    public override void Fire(Transform player)
    {
        if (Time.time < nextFire) return;

        //fire a projectile
        Vector2 dir = CameraReference.MouseVec(player.position);
        var fire = GameObject.Instantiate(fireball, player.position, Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x)));
        Fireball bullet = fire.GetComponent<Fireball>();
        bullet.Initialize(damage, spd);
        fire.layer = player.gameObject.layer;
        bullet.fuse = Time.time + dir.magnitude / spd;
        bullet.explosionDamage = explosionDamage;
        bullet.explosionForce = explosionForce;
        bullet.explosionRadius = explosionRadius;

        nextFire = Time.time + fireTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Spell() : base(swordMass)
    {

    }
}

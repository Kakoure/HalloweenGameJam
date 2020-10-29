using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sword : Weapon
{
    public static int swordMass = 10;

    public float radius;
    public float arcRadians; //sq
    public int damage;
    public float force;

    private Sprite _sprite;
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
        SetUseTime();
    }

    private FireProjectile throwitem;

    // Start is called before the first frame update
    void Start()
    {
        throwitem = FireProjectile.ThrowProjectile(Mass, 0.5f);
        _sprite = GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sword() : base(swordMass)
    {

    }
}

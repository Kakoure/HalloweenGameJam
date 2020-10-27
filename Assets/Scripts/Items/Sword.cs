using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sword : Item
{
    static int swordMass = 1;

    public float radius;
    public float arcRadians; //sq
    public int damage;
    public float force;
    public float swingTime;

    private Sprite _sprite;
    private float nextSwing = 0;
    public override Sprite Sprite => _sprite;

    public override void AltFire(Transform player)
    {
        throw new System.NotImplementedException();
    }

    public override void Fire(Transform player)
    {
        if (Time.time < nextSwing) return;
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
                    entity.ApplyImpulse(force, player.position);
                    entity.DealDamage(damage);
                }
            }
        }
        nextSwing = Time.time + swingTime;
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

    public Sword() : base(swordMass)
    {

    }
}

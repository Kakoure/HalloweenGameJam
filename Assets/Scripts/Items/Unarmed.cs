using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Unarmed : Weapon
    {
        public float radius;
        public float arcRadians; //sq
        public int damage;
        public float force;

        private Sprite _sprite;
        public override Sprite Sprite => _sprite;

        private void Start()
        {
            _sprite = GetComponent<SpriteRenderer>().sprite;
        }

        public override void AltFire(Transform player, bool down)
        {
            if (down)
                Debug.Log("You misplace your weapon");
        }

        public override void Fire(Transform player, bool down)
        {
            if (!down) return;
            if (!IsReady) return;

            Debug.Log("You swing your fists");

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
            SetUseTime();
        }

        public Unarmed() : base(1)
        {

        }
    }
}

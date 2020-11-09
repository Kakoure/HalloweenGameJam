using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CooldownTimer;
using UnityEngine.UI;
using static Boomerang;

namespace Items
{
    [CreateAssetMenu]
    public class Sword : Weapon
    {
        public static ItemID id = ItemID.Sword;
        public override ItemID ID => id;
        public static string itemName = "Sword";
        public override string Name => itemName;
        private static Sprite sprite;
        public override Sprite Sprite => sprite;

        public static int swordMass = 10;

        public float radius;
        public float arcRadians; //sq
        public int damage;
        public float force;
        public float lungeForce;

        //fix fireprojectile
        public FireProjectile throwitem;


        private Cooldown comboReset;

        //make static member of weapon
        Converter lungeConverter = f => f < .1f ? 0f : Mathf.Max(0, (2 - Mathf.Pow((f + .5f), 2)));

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
                bullet.GetComponent<Animator>().enabled = false;
                Inventory.Instance.shield = null;
            }
        }

        public override void Fire(Transform player, bool down)
        {
            if (!down) return;
            if (!IsReady) return;
            if (!Player.Instance.playerMove.PathEnd) return;

            //Attack sequence shows up as "Unused" (also uses reflection to find the method)
            //Owner.StartCoroutine("AttackSequence"); 
            Owner.StartCoroutine(AttackSequence());
            SetUseTime();
        }

        //delete
        void Awake()
        {
            //replace _sprite
            //_sprite = Owner.GetComponent<SpriteRenderer>().sprite;
        }

        public Sword() : base(swordMass)
        {

        }

        //TODO: fix the arc scan?
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
            }
            else if (anim.GetInteger("Combo") == 0)
            {
                anim.SetInteger("Combo", 1);
            }
            else
            {
                anim.SetInteger("Combo", 0);
            }
            comboReset.Use(1f);
            anim.SetFloat("xInput", lookDir.x);
            anim.SetFloat("yInput", lookDir.y);

            Path lungePath = LinePath(lungeConverter, lookDir);
            Player.Instance.playerMove.SetPath(Boomerang.Mult(Player.Instance.playerMove.speed, lungePath), .5f);



            yield return new WaitForSeconds(.2f);
            HitScan();
            yield return new WaitForSeconds(.2f);
            HitScan();
        }
    }
}
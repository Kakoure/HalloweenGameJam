using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    public interface IAbility
    {
        Bullet Execute(Transform t, out Vector2 v);
    }

    //FIX FIREPROJECTILE
    [System.Serializable]
    public class FireProjectile : IAbility
    {
        public int damage;
        public float speed;
        public float throwTime;
        public float knockBack;

        public GameObject Bullet;

        public Bullet Execute(Transform player, out Vector2 dir)
        {
            dir = CameraReference.MouseVec(player.position);
            return Execute(player, dir);
        }
        public Bullet Execute(Transform player, Vector2 dir)
        {
            var fire = GameObject.Instantiate(Bullet, player.position, Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x)));
            Bullet bullet = fire.GetComponent<Bullet>();
            bullet.Initialize(damage, speed);
            bullet.maxTime = Time.time + throwTime;
            fire.layer = player.gameObject.layer;
            bullet.knockBack = knockBack;

            return bullet;
        }
        
        public FireProjectile(GameObject original,int damage, float knockBack, float speed, float throwTime = Mathf.Infinity)
        {
            //if (original == null) original = CameraReference.Instance.bulletGeneric;
            this.Bullet = original;
            this.damage = damage;
            this.speed = speed;
            this.throwTime = throwTime;
            this.knockBack = knockBack;
        }

        public static FireProjectile ThrowProjectile(int mass, float throwTime)
        {
            return new FireProjectile(CameraReference.Instance.bulletGeneric, mass, Item.kbConst * Mathf.Sqrt(mass), Item.massConstant / mass);
        }
    }
}

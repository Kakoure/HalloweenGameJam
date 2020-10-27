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
    public class FireProjectile : IAbility
    {
        int damage;
        float speed;
        float throwTime;

        public GameObject Bullet;

        public Bullet Execute(Transform player, out Vector2 dir)
        {
            dir = CameraReference.MouseVec(player.position);
            var fire = GameObject.Instantiate(Bullet, player.position, Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x)));
            Bullet bullet = fire.GetComponent<Bullet>();
            bullet.Initialize(damage, speed);
            bullet.maxTime = Time.time + throwTime;
            fire.layer = player.gameObject.layer;
            return bullet;
        }
        
        public FireProjectile(GameObject original,int damage, float speed, float throwTime = Mathf.Infinity)
        {
            if (original == null) original = CameraReference.Instance.bulletGeneric;
            this.Bullet = original;
            this.damage = damage;
            this.speed = speed;
            this.throwTime = throwTime;
        }
    }
}

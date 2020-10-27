using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class StrawberryJam : Item
    {
        public static int itemMass = 1;

        public Sprite sprite;
        public override Sprite Sprite => sprite;
        public FireProjectile throwJar;

        public void Start()
        {
            throwJar = new FireProjectile(null, itemMass, Mathf.Sqrt(massConstant / itemMass), .5f);
        }


        public override void AltFire(Transform player)
        {
            var projectile = throwJar.Execute(player, out _);
            projectile.GetComponent<SpriteRenderer>().sprite = this.sprite;
            projectile.onCollision = () => DropAt(projectile.transform.position);
            Inventory.PopFromSlot(Inventory.Instance.shield);
        }
        public override void Fire(Transform player)
        {
            Debug.Log("You begin eating the Strawberry jam");
        }

        public StrawberryJam() : base(itemMass)
        {

        }
    }
}
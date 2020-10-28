using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UsePrimary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool? mouse = null;
    bool? mouse2 = null;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) mouse = true;
        if (Input.GetButtonDown("Fire2")) mouse2 = true;
        if (Input.GetButtonUp("Fire1")) mouse = false;
        if (Input.GetButtonUp("Fire2")) mouse2 = false;
        if (mouse != null)
        {
            //use weapon
            Item primary = Inventory.Instance.weapon.Item;
            primary?.Fire(this.transform, (bool)mouse);
            mouse = null;
        }
        if(mouse2 != null)
        {
            //use Secondary
            Item offhand = Inventory.Instance.shield.Item;
            offhand?.AltFire(this.transform, (bool)mouse2);
            mouse2 = null;
        }
    }
}

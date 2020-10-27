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

    bool mouse = false;
    bool mouse2 = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) mouse = true;
        if (Input.GetButtonDown("Fire2")) mouse2 = true;
        if (mouse)
        {
            //use weapon
            Item primary = Inventory.Instance.weapon.Item;
            primary?.Fire(this.transform);
            mouse = false;
        }
        if(mouse2)
        {
            //use Secondary
            Item offhand = Inventory.Instance.shield.Item;
            offhand?.AltFire(this.transform);
            mouse2 = false;
        }
    }
}

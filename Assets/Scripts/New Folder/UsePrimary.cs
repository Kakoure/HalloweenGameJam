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
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) mouse = true;
        if (mouse)
        {
            Debug.Log("mouse detec");
            //use weapon
            Item primary = Inventory.Instance.weapon.Item;
            primary?.Fire(this.transform);
            mouse = false;
        }
    }
}

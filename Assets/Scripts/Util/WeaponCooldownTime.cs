using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCooldownTime : MonoBehaviour
{
    Text t;
    string msg;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
        msg = t.text;
    }

    // Update is called once per frame
    void Update()
    {
        Item i = Inventory.CurrentWeapon;
        Weapon w = i as Weapon;
        if (w != null)
        {
            if (w.IsReady)
            {
                t.text = msg + "Ready";
            }
            else
            {
                t.text = msg + w.TimeRemaining();
            }
        }
    }
}

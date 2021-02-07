using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//TODO: merge this with PlayerMove
public class UsePrimary : MonoBehaviour
{
    Player p;

    // Start is called before the first frame update
    void Start()
    {
        p = Player.Instance;
    }

    bool? mouseButton = null;
    bool? mouseButton2 = null;
    bool pickupItem;
    // Update is called once per frame
    void Update()
    {
        //Pause guard
        if (PauseController.Paused) return;

        //Path overrides control
        if (!p.playerMove.PathEnd) return;

        //Assigning controls
        if (Input.GetButtonDown("Fire1")) mouseButton = true;
        if (Input.GetButtonDown("Fire2")) mouseButton2 = true;
        if (Input.GetButtonUp("Fire1")) mouseButton = false;
        if (Input.GetButtonUp("Fire2")) mouseButton2 = false;

        pickupItem = Input.GetButton("Pickup Item");

        //resolve controls
        if (mouseButton != null)
        {
            //use weapon
            Item primary = Inventory.CurrentWeapon;
            primary?.Fire(this.transform, (bool)mouseButton);
            mouseButton = null;
        }
        if (mouseButton2 != null)
        {
            //use Secondary
            Item offhand = Inventory.Instance.shield.Item;
            offhand?.AltFire(this.transform, (bool)mouseButton2);
            mouseButton2 = null;
        }
        if (pickupItem)
        {
            //if an Item is highlighted, pick it up.
            Player.Instance.itemPickupTarget?.PickUpItem();

            /*
             * It will sort itself out next update.
            Player.Instance.itemPickupTarget = null;
            */
        }
    }
}

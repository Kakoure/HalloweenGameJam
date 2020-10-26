using Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //a single inventory slot
    [System.Serializable]
    public class InventorySlot
    {
        //[NonSerialized]
        public Sprite standardSprite;
        [SerializeField]
        public Image img;
        [SerializeField]
        private Item item;

        public Item Item 
        { 
            get
            {
                return item;
            }
            set
            {
                if (value == null) img.sprite = standardSprite;
                else img.sprite = value.Sprite;
                this.item = value;
            }
        }
    }

    public static Inventory Instance { get; private set; }

    public static void AssignTo(Item item, int slot)
    {
        AssignTo(item, Instance.inventorySlots[slot]);
    }
    public static void AssignTo(Item item, InventorySlot slot)
    {
        slot.Item = item;
    }

    public static Item PopFromSlot(InventorySlot slot)
    {
        Item tmp = slot.Item;
        slot.Item = null;
        return tmp;
    }
    public static Item PopFromSlot(Transform slot)
    {
        InventorySlot invSlot = Instance.FindSlot(slot);
        return PopFromSlot(invSlot);
    }

    public static int GetOpenSlot()
    {
        int i = Instance.inventorySlots.Length;

        //lol
        while(i --> 0)
        {
            if (Instance.inventorySlots[i].Item == null) break;
        }
        return i;
    }

    public void Swap(Transform t1, Transform t2)
    {
        if (t1 == t2) return;
        InventorySlot slot1;
        InventorySlot slot2;
        slot1 = FindSlot(t1);
        slot2 = FindSlot(t2);
        if (slot1 == null) return;
        if (slot2 == null) return;

        Item i1 = PopFromSlot(slot1);
        Item i2 = PopFromSlot(slot2);
        AssignTo(i1, slot2);
        AssignTo(i2, slot1);
    }

    public InventorySlot FindSlot(Transform t)
    {
        if (t == shield.img.transform) return shield;
        if (t == weapon.img.transform) return weapon;
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].img.transform == t) return inventorySlots[i];
        }
        return null;
    }

    [SerializeField]
    private InventorySlot[] inventorySlots;
    public InventorySlot weapon;
    public InventorySlot shield;

    private void Awake()
    {
        if (Instance != null) Debug.LogError("Two instances of the Inventory detected");
        Instance = this;

        for (int i = 0; i < inventorySlots.Length; i++)
        { 
            inventorySlots[i].standardSprite = inventorySlots[i].img.sprite;
        }
        weapon.standardSprite = weapon.img.sprite;
        shield.standardSprite = shield.img.sprite;
    }

    private void Start()
    {
    }
}


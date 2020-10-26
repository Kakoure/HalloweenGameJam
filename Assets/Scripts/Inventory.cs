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
    public struct InventorySlot
    {
        public Image img;
        public Item item;
    }

    public static Inventory Instance { get; private set; }

    public static void AssignTo(Item item, int slot)
    {
        Instance.inventorySlot[slot].item = item;
        Instance.inventorySlot[slot].img.sprite = item.Sprite;
    }

    public static Item PopFromSlot(int slot)
    {
        Item tmp = Instance.inventorySlot[slot].item;
        Instance.inventorySlot[slot].item = null;
        Instance.inventorySlot[slot].img.sprite = null;
        return tmp;
    }

    [SerializeField]
    private InventorySlot[] inventorySlot;

    private void Start()
    {
        if (Instance != null) Debug.LogError("Two instances of the Inventory detected");
        Instance = this;
    }
}


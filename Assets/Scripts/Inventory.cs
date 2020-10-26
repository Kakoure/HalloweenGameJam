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
        Sprite sprite;
        Instance.inventorySlots[slot].item = item;
        if (item == null) sprite = null;
        else sprite = item.Sprite;
        Instance.inventorySlots[slot].img.sprite = sprite;
    }

    public static Item PopFromSlot(int slot)
    {
        Item tmp = Instance.inventorySlots[slot].item;
        Instance.inventorySlots[slot].item = null;
        Instance.inventorySlots[slot].img.sprite = null;
        return tmp;
    }

    public void Swap(Transform t1, Transform t2)
    {
        Debug.Log(t1);

        int slot1;
        int slot2;
        slot1 = FindSlot(t1);
        slot2 = FindSlot(t2);
        if (slot1 == -1) return;
        if (slot2 == -1) return;

        Item i1 = PopFromSlot(slot1);
        Item i2 = PopFromSlot(slot2);
        AssignTo(i1, slot2);
        AssignTo(i2, slot1);
    }

    public int FindSlot(Transform t)
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].img.transform == t) return i;
        }
        return -1;
    }

    [SerializeField]
    private InventorySlot[] inventorySlots;

    private void Start()
    {
        if (Instance != null) Debug.LogError("Two instances of the Inventory detected");
        Instance = this;
    }
}


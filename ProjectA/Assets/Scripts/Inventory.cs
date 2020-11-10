using Items;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Inventory.InventorySlot;

public partial class Inventory : MonoBehaviour
{
    //a single inventory slot
    [System.Serializable]
    public class InventorySlot
    {
        public enum SlotType
        { 
            Inventory = 0,
            Shield,
            Weapon,
        }

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

        [NonSerialized]
        public SlotType slotType = SlotType.Inventory;
    }

    public static Inventory Instance { get; private set; }

    //replace these with methods
    private static void AssignTo(Item item, int slot)
    {
        AssignTo(item, Instance.inventorySlots[slot]);
    }
    private static void AssignTo(Item item, InventorySlot slot)
    {
        slot.Item = item;
        EvaluateAnimWeaponID();
        Player.Instance.PlaySound(Player.Instance.equipSound);
    }

    //replace with methods
    private static Item PopFromSlot(InventorySlot slot)
    {
        Item tmp = slot.Item;
        slot.Item = null;
        EvaluateAnimWeaponID();
        return tmp;
    }
    private static Item PopFromSlot(Transform slot)
    {
        InventorySlot invSlot = Instance.FindSlot(slot);
        return PopFromSlot(invSlot);
    }

    public static int GetEmptyInventorySlot() => GetEmptyInventorySlot(Instance.inventorySlots.Length);
    public static int GetEmptyInventorySlot(int upper)
    {
        while(upper --> 0)
        {
            if (Instance.inventorySlots[upper].Item == null)
                break;
        }
        return upper;
    }
    public static InventorySlot GetOpenSlot()
    {
        if (Instance.weapon.Item == null) return Instance.weapon;
        if (Instance.shield.Item == null) return Instance.shield;

        InventorySlot slot = null;
        int i = Instance.inventorySlots.Length;

        //lol
        while(i --> 0)
        {
            if (Instance.inventorySlots[i].Item == null)
            {
                slot = Instance.inventorySlots[i];
                break;
            }
        }
        return slot;
    }

    //index is only used if slotType is Inventory
    public static InventorySlot GetSlot(SlotType slotType, int index = 0)
    {
        switch (slotType)
        {
            case SlotType.Inventory:
                return Instance.inventorySlots[index];
            case SlotType.Shield:
                return Instance.shield;
            case SlotType.Weapon:
                return Instance.weapon;
            default:
                throw new ArgumentException();
        }
    }

    public static Item CurrentWeapon => Instance.weapon.Item != null? Instance.weapon.Item : Instance.unarmed;

    public static bool Swap(Transform t1, Transform t2)
    {
        if (t1 == t2) return false;
        InventorySlot slot1;
        InventorySlot slot2;
        slot1 = Instance.FindSlot(t1);
        slot2 = Instance.FindSlot(t2);
        if (slot1 == null) return false;
        if (slot2 == null) return false;

        return Swap(slot1, slot2);
    }
    public static bool Swap(InventorySlot slot1, InventorySlot slot2)
    {
        Item i1 = slot1.Item;
        Item i2 = slot2.Item;

        bool b1 = true, b2 = true;
        Action a1 = null, a2 = null;
        i1?.SwapSlot(i1)(slot1, slot2, out b1, out a1);
        i2?.SwapSlot(i1)(slot2, slot1, out b2, out a2);

        if (b1 && b2)
        {
            //swap slots
            PopFromSlot(slot1);
            PopFromSlot(slot2);
            AssignTo(i1, slot2);
            AssignTo(i2, slot1);

            //finalize
            a1?.Invoke();
            a2?.Invoke();

            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool Pickup(InventorySlot slot, Item item)
    {
        if (slot.Item != null) return false;

        item.SwapSlot(item)(null, slot, out bool success, out Action f1);

        if (success)
        {
            AssignTo(item, slot);

            f1?.Invoke();

            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool Drop(InventorySlot slot, Vector2 position)
    {
        slot.Item.SwapSlot(slot.Item)(slot, null, out bool success, out Action finalize);

        if (success)
        {
            Item i = PopFromSlot(slot);
            i.DropAt(position);
            finalize?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
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
    private Item unarmed;
    public InventorySlot weapon;
    public InventorySlot shield;
    private ItemID currentWepID;

    private void Awake()
    {
        if (Instance != null) Debug.LogError("Two instances of the Inventory detected");
        Instance = this;

        //initialize InventorySlots
        for (int i = 0; i < inventorySlots.Length; i++)
        { 
            inventorySlots[i].standardSprite = inventorySlots[i].img.sprite;
        }
        weapon.standardSprite = weapon.img.sprite;
        weapon.slotType = InventorySlot.SlotType.Weapon;
        shield.standardSprite = shield.img.sprite;
        shield.slotType = InventorySlot.SlotType.Shield;
    }

    private void Start()
    {
        //find the unarmed script by getting the ItemGeneric first
        unarmed = GameObject.FindGameObjectWithTag("Unarmed").GetComponent<ItemGeneric>().itemObject;
    }

    //TODO make this a method of Item
    private static void EvaluateAnimWeaponID()
    {
        int swordID = ItemID.GetID<Sword>().ID;
        int bowID = ItemID.GetID<Bow>().ID;
        Animator anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        int weaponID = ItemID.GetID(CurrentWeapon.GetType());
        if (anim != null && Instance.currentWepID != weaponID)
        {
            if (weaponID == swordID)
            {
                anim.runtimeAnimatorController = Resources.Load("Player/Animations/Player Sword Anim Controller") as RuntimeAnimatorController;
            }
            else if (weaponID == bowID)
            {
                anim.runtimeAnimatorController = Resources.Load("Player/Animations/Player Bow Anim Controller") as RuntimeAnimatorController;
            }
            else
            {
                anim.runtimeAnimatorController = Resources.Load("Player/Animations/Player Anim Controller") as RuntimeAnimatorController;
            }
        }
        Instance.currentWepID = ItemID.GetID(CurrentWeapon.GetType());
    }
}


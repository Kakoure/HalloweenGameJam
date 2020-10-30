using Items;
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
        EvaluateAnimWeaponID();
    }

    public static Item PopFromSlot(InventorySlot slot)
    {
        Item tmp = slot.Item;
        slot.Item = null;
        EvaluateAnimWeaponID();
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

    public static Item CurrentWeapon => Instance.weapon.Item ?? Instance.unarmed;

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
    private Item unarmed;
    public InventorySlot weapon;
    public InventorySlot shield;
    private int currentWepID;

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
        unarmed = GameObject.FindGameObjectWithTag("Unarmed").GetComponent<Item>();
    }

    private static void EvaluateAnimWeaponID()
    {
        Animator anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        if(anim != null && Instance.currentWepID != CurrentWeapon.ID)
        {
            switch (CurrentWeapon.ID)
            {
                case 1:
                    anim.runtimeAnimatorController = Resources.Load("Player/Animations/Player Sword Anim Controller") as RuntimeAnimatorController;
                    break;
                default:
                    anim.runtimeAnimatorController = Resources.Load("Player/Animations/Player Anim Controller") as RuntimeAnimatorController;
                    break;
            }
           
        }
        Instance.currentWepID = CurrentWeapon.ID;
    }
    private void Start()
    {
    }
}


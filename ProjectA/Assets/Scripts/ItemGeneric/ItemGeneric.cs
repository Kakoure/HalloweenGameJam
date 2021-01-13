using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class ItemGeneric : MonoBehaviour, IClickable
{
    public static List<ItemGeneric> GetNearbyItems(Vector2 position, float radius)
    {
        var colliders = UnityEngine.Physics2D.OverlapCircleAll(position, radius, LayerMask.GetMask("Items"));
        List<ItemGeneric> items = new List<ItemGeneric>(colliders.Length);
        foreach (var col in colliders)
        {
            items.Add(col.GetComponent<ItemGeneric>());
        }
        return items;
    }
    public static ItemGeneric GetNearestItem(Vector2 position, float radius)
    {
        var items = GetNearbyItems(position, radius);

        if (items.Count == 0) return null;

        Func<ItemGeneric, float> getRad = (item) => ((Vector2)item.transform.position - position).sqrMagnitude;

        ItemGeneric nearest = items[0];
        float minDist = getRad(nearest);
        for (int i = 1; i < items.Count; i++)
        {
            var item = items[i];
            float dist = getRad(item);
            if (dist < minDist)
            {
                //replace nearest
                nearest = item;
                minDist = dist;
            }
        }
        return nearest;
    }

    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
            return _spriteRenderer;
        }
    }

    //find better names for these
    public MonoScript itemScript;
    public Item itemObject;

    //start is called before first update while awake is called the moment the object is created
    private void Awake()
    {
        //to make sure that at runtime, each itemObject is unique, but in the editor, each item has the same "data"
        if (itemObject != null)
        {
            Item gameClone = Instantiate<Item>(itemObject);
            itemObject = gameClone;
        }
        else
        {
            //I can assume that itemObject will be assigned by the time i reach Start()?
        }
    }
    private void Start()
    {
        //error msg
        if (itemObject == null)
        {
            Debug.LogError("ItemGeneric missing item object");
            return;
        }

        this.SpriteRenderer.sprite = itemObject.Sprite;
        this.SpriteRenderer.material.SetColor("_HighlightColor", new Color(0, 0, 0, 0));

        itemObject.Owner = this;
        itemObject.Initialize();
    }
    public void OnClick()
    {
        //just pass the method over to the itemObject
        itemObject.OnClick();
    }
}

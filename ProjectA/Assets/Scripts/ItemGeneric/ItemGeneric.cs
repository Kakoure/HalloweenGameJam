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

        itemObject.Owner = this;
        itemObject.Initialize();
    }
    public void OnClick()
    {
        //just pass the method over to the itemObject
        itemObject.OnClick();
    }
}

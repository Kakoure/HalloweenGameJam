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
    //find better names for these
    public MonoScript itemScript;
    public Item itemObject;

    private void Awake()
    {
        //to make sure that at runtime, each itemObject is unique, but in the editor, each item has the same "data"
        Item gameClone = Instantiate<Item>(itemObject);
        itemObject = gameClone;
        itemObject.Owner = this;
    }
    private void Start()
    {
        itemObject.Initialize();
    }
    public void OnClick()
    {
        //just pass the method over to the itemObject
        itemObject.OnClick();
    }
}

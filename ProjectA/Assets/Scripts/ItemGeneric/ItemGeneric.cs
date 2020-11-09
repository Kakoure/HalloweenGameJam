using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class ItemGeneric : MonoBehaviour
{
    //find better names for these
    public MonoScript itemScript;
    public Item itemObject;

    private void Start()
    {
        //to make sure that at runtime, each itemObject is unique, but in the editor, each item has the same "data"
        Item gameClone = Instantiate<Item>(itemObject);
        itemObject = gameClone;
    }
}

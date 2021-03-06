﻿using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.PlayerLoop;

//learning to make a custom Unity editor was not easy. I really hope that this was worth it
[CustomEditor(typeof(ItemGeneric))]
[CanEditMultipleObjects]
class ItemGenericEditor : Editor
{
    SerializedProperty serializedItemProperty;
    SerializedProperty itemProperty;
    bool IsValid => itemGeneric.itemObject != null;
    string itemName = "New Item Name";

    ItemGeneric itemGeneric => serializedObject.targetObject as ItemGeneric;

    void OnEnable()
    {
        serializedItemProperty = serializedObject.FindProperty("itemScript");
        itemProperty = serializedObject.FindProperty("itemObject");
    }

    //changing the item should change the sprite renderer.
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(serializedItemProperty);

        if (EditorGUI.EndChangeCheck())
        {
            UpdateSpriteRenderer();
        }

        EditorGUILayout.PropertyField(itemProperty);
        serializedObject.ApplyModifiedProperties();

        GUILayout.Space(15);
        GUILayout.Label("Create New Item:");

        itemName = GUILayout.TextField(itemName);
        if (GUILayout.Button("Create new"))
        {
            //create a new instance of the Item
            Type itemType = GetItemType();
            var item = CreateInstance(itemType);
            string itemFolder = Item.itemsDirectoryAssets + Item.GetItemName(itemType);
            if (!AssetDatabase.IsValidFolder(itemFolder))
            {
                AssetDatabase.CreateFolder($"Assets/Resources/Data/Items", Item.GetItemName(itemType));
            }
            
            AssetDatabase.CreateAsset(item, itemFolder + $"/{itemName}.asset");
            itemGeneric.itemObject = item as Item;
        }

        serializedObject.ApplyModifiedProperties();
    }

    Type GetItemType()
    {
        MonoScript script = itemGeneric.itemScript;

        //if the script is null then no script was selected
        if (script == null)
        {
            itemGeneric.itemObject = null;
            return null;
        }

        //what is this? java?
        Type itemType = script.GetClass();

        if (itemType.IsAbstract || !typeof(Item).IsAssignableFrom(itemType))
        {
            //the type is either abstract or does not inherit from Item
            UnityEngine.Debug.LogError($"The Script {script.name} does not represent a valid Item");

            itemGeneric.itemObject = null;
            return null;
        }

        return itemType;
    }
    //called when the item property is changed
    private void UpdateSpriteRenderer()
    {
        Type itemType = GetItemType();
        if (itemType == null)
        {
            //default sprite is sword
            itemGeneric.SpriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(Item.itemsDirectoryAssets + "Sword" + "/Sprite.png");
            return;
        }

        //item is a valid item type
        //string dir = Item.GetItemPath(itemID);
        string dir = Item.itemsDirectoryAssets;
        dir += Item.GetItemName(itemType) + "/";

        //load the appropriate sprite if it exists
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(dir + "Sprite.png");
        if (sprite != null)
        {
            itemGeneric.SpriteRenderer.sprite = sprite;
        }
        else
        {
            //default sprite is sword
            Debug.LogError($"sprite at {dir + "Sprite.png"} not found");
            itemGeneric.SpriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(Item.itemsDirectoryAssets + "Sword" + "/Sprite.png");
        }   
    }
}
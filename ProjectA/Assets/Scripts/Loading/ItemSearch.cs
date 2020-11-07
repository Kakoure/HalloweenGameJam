﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.WSA;

namespace Items
{
    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class LoadResourceAttribute: Attribute
    {
        Action<UnityEngine.Object> assigner;
        Type type;
        string name;

        //folderPath ends with /
        public void Load(string folderPath)
        {
            UnityEngine.Object o = Resources.Load(folderPath + name, type);
            assigner(o);
        }

        public LoadResourceAttribute(string name, Type systemTypeInstance, Action<UnityEngine.Object> assigner)
        {
            this.assigner = assigner;
            this.type = systemTypeInstance;
            this.name = name;
        }
    }

    partial class Item
    {
        //used for loading screen
        static readonly Type[] itemList = new Type[(int)ItemID.SIZE];

        //given ItemID, should be able to find item name
        static readonly string[] itemNames = new string[(int)ItemID.SIZE];

        //directory of all items - ends with a /
        static readonly string itemsDirectory = "Data/Items/";

        static readonly string nameField = "itemName";
        static readonly string idField = "id";

        //since the game jam is over, im allowed to write clean code
        public static void LoadItems()
        {
            //fill up the nameDirectory using reflection
            var mods = Assembly.GetExecutingAssembly().Modules;
            foreach (var m in mods)
            {
                TypeFilter isAnItem = (i, o) => typeof(Item).IsAssignableFrom(i) && !i.IsAbstract;
                
                var types = m.FindTypes(isAnItem, null);
                foreach (var type in types)
                {
                    var name = type.GetField(nameField, BindingFlags.Public | BindingFlags.Static);
                    var id = type.GetField(idField, BindingFlags.Public | BindingFlags.Static);
                    if (name == null)
                    {
                        Debug.LogError($"field {nameField} of {type} is missing");
                        continue;
                    }
                    else if (id == null)
                    {
                        Debug.LogError($"field {idField} of {type} is missing");
                        continue;
                    }
                    else
                    {
                        //item with valid name and ID
                        //any item without a valid name and (net) id will not be loaded
                        int itemID = (int)id.GetValue(null);
                        string itemName = (string)name.GetValue(null);
                        if (itemList[itemID] != null)
                        {
                            Debug.LogError($"itemID {(ItemID)itemID} has a clash between {itemName} and {itemNames[itemID]}");
                            continue;
                        }
                        itemList[itemID] = type;
                        itemNames[itemID] = itemName;
                        //nameDictionary.Add((ItemID)id.GetValue(null), (string)name.GetValue(null));
                    }
                }
            }

            LoadItemResources();
        }
        private static void LoadItemResources()
        {
            for (int id = 0; id < (int)ItemID.SIZE; id++)
            {
                Type item = itemList[id];

                if (item == null) continue;
                var attributes = item.GetCustomAttributes<LoadResourceAttribute>();

                foreach (var att in attributes)
                {
                    //load resources according to the LoadResource attributes
                    string dataDirectory = GetItemPath(id);

                    att.Load(dataDirectory);
                }
            }
        }

        // end with a /
        public static string GetItemPath(ItemID id)
        {
            return itemsDirectory + itemNames[(int)id] + "/";
        }
        public static string GetItemPath(int id) => GetItemPath((ItemID)id);
    }
}
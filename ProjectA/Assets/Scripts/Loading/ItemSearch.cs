using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.WSA;

namespace Items
{
    partial class Item
    {
        //used for loading screen
        //TODO: Use List<T> instead
        public static Type[] itemList = null;

        //given ItemID, should be able to find item name
        public static string[] itemNames = null;

        //start from Assets
        //directory of all items - ends with a /
        public static readonly string itemsDirectoryAssets = "Assets/Resources/Data/Items/";

        //start from Resources
        public static readonly string itemsDirectoryResources = "Data/Items/";

        static readonly string nameField = "itemName";
        [Obsolete]
        static readonly string idField = "id";

        //since the game jam is over, im allowed to write clean code
        public static void LoadItems()
        {
            //fill up the nameDirectory using reflection
            var mods = Assembly.GetExecutingAssembly().Modules;
            foreach (var m in mods)
            {
                TypeFilter isAnItem = (i, o) => typeof(Item).IsAssignableFrom(i) && !i.IsAbstract;
                
                //initialize itemNames and itemList
                itemList = m.FindTypes(isAnItem, null);
                itemNames = new string[itemList.Length];

                for (int i = 0; i < itemList.Length; i++) 
                {
                    Type type = itemList[i];
                    var name = type.GetField(nameField, BindingFlags.Public | BindingFlags.Static);
                    //id is no longer needed
                    if (name == null)
                    {
                        UnityEngine.Debug.LogError($"field {nameField} of {type} is missing");
                        continue;
                    }
                    else
                    {
                        //item with valid name
                        string itemName = (string)name.GetValue(null);
                        itemList[i] = type;
                        itemNames[i] = itemName;
                        //nameDictionary.Add((ItemID)id.GetValue(null), (string)name.GetValue(null));
                    }
                }
            }

            LoadItemResources();
        }
        private static void LoadItemResources()
        {
            //TODO: find a better way to itterate through IDs
            for (int id = 1; id <= itemList.Length; id++)
            {
                Type item = itemList[id - 1];

                if (item == null) continue;
                var attributes = item.GetCustomAttributes<LoadingAttribute>(true);
                
                //load resources according to the LoadResource attributes
                string dataDirectory = GetItemPath(id);

                foreach (var att in attributes)
                {
                    //load the resource
                    att.Load(dataDirectory, item);
                }
            }
        }

        [Obsolete]
        public static ItemIDObsolete GetItemID(Type item)
        {
            if (typeof(Item).IsAssignableFrom(item))
            {
                return (ItemIDObsolete)item.GetField(idField, BindingFlags.Public | BindingFlags.Static).GetValue(null);
            }
            else return ItemIDObsolete.Error;
        }

        public static string GetItemName(Type item)
        {
            if (typeof(Item).IsAssignableFrom(item))
            {
                return (string)item.GetField(nameField, BindingFlags.Public | BindingFlags.Static).GetValue(null);
            }
            else return null;
        }

        // end with a /
        [Obsolete]
        public static string GetItemPath(ItemIDObsolete id)
        {
            return itemsDirectoryResources + itemNames[(int)id] + "/";
        }
        public static string GetItemPath(ItemID id)
        {
            return itemsDirectoryResources + id.GetItemName() + "/";
        }
    }
}

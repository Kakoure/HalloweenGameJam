using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class LoadResourceToFieldAttribute : Attribute
    {
        Type type;
        string key;
        string staticFieldName;

        //folderPath ends with /
        public void Load(string folderPath, Type item)
        {
            //get the resource from the file system
            UnityEngine.Object o = Resources.Load(folderPath + key, type);

            //assign the resource to the static field
            var field = item.GetField(staticFieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            //since field is static this line should not throw null reference.
            //however, if the resource type is wrong, then this line may very well throw an exception
            field.SetValue(null, o);
        }

        /// <summary>
        /// assigns a static field in the loading screen (field must inherit from UnityEngine.Object)
        /// (Unfortunately Attributes cannot be generic which is dumb)
        /// </summary>
        /// <param name="staticFieldName">name of the field on the item (just the simple name)</param>
        /// <param name="key">name of the resource in the Unity filesystem</param>
        public LoadResourceToFieldAttribute(string staticFieldName, string key) 
            : this(staticFieldName, key, typeof(UnityEngine.Object)) { }

        /// <summary>
        /// assigns a static field in the loading screen (field must inherit from UnityEngine.Object)
        /// (Unfortunately Attributes cannot be generic which is dumb)
        /// </summary>
        /// <param name="staticFieldName">name of the field on the item (just the simple name)</param>
        /// <param name="key">name of the resource in the Unity filesystem</param>
        /// <param name="resourceType">type of resource used for redundancy</param>
        public LoadResourceToFieldAttribute(string staticFieldName, string key, Type resourceType)
        {
            this.type = resourceType;
            this.key = key;
            this.staticFieldName = staticFieldName;
        }
    }

    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class LoadResourceToFieldInheritedAttribute : Attribute
    {
        Type type;
        string key;
        string staticFieldName;

        //folderPath ends with /
        public void Load(string folderPath, Type item)
        {
            //get the resource from the file system
            UnityEngine.Object o = Resources.Load(folderPath + key, type);

            //assign the resource to the static field
            var field = item.GetField(staticFieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            //since field is static this line should not throw null reference.
            //however, if the resource type is wrong, then this line may very well throw an exception
            field.SetValue(null, o);
        }

        /// <summary>
        /// assigns a static field in the loading screen (field must inherit from UnityEngine.Object)
        /// (Unfortunately Attributes cannot be generic which is dumb)
        /// </summary>
        /// <param name="staticFieldName">name of the field on the item (just the simple name)</param>
        /// <param name="key">name of the resource in the Unity filesystem</param>
        public LoadResourceToFieldInheritedAttribute(string staticFieldName, string key)
            : this(staticFieldName, key, typeof(UnityEngine.Object)) { }

        /// <summary>
        /// assigns a static field in the loading screen (field must inherit from UnityEngine.Object)
        /// (Unfortunately Attributes cannot be generic which is dumb)
        /// </summary>
        /// <param name="staticFieldName">name of the field on the item (just the simple name)</param>
        /// <param name="key">name of the resource in the Unity filesystem</param>
        /// <param name="resourceType">type of resource used for redundancy</param>
        public LoadResourceToFieldInheritedAttribute(string staticFieldName, string key, Type resourceType)
        {
            this.type = resourceType;
            this.key = key;
            this.staticFieldName = staticFieldName;
        }
    }

    //attributes for properties
    //must be at least set property
    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class LoadResourceToPropertyAttribute : Attribute
    {
        Type type;
        string key;
        string staticPropertyName;

        //folderPath ends with /
        public void Load(string folderPath, Type item)
        {
            //get the resource from the file system
            UnityEngine.Object o = Resources.Load(folderPath + key, type);

            //assign the resource to the static field
            var property = item.GetProperty(staticPropertyName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.SetProperty);

            //property is guarenteed to be static and set
            //however, if the resource type is wrong, then this line may very well throw an exception
            property.SetValue(null, o);
        }

        /// <summary>
        /// assigns a static field in the loading screen (field must inherit from UnityEngine.Object)
        /// (Unfortunately Attributes cannot be generic which is dumb)
        /// </summary>
        /// <param name="staticFieldName">name of the field on the item (just the simple name)</param>
        /// <param name="key">name of the resource in the Unity filesystem</param>
        public LoadResourceToPropertyAttribute(string staticFieldName, string key)
            : this(staticFieldName, key, typeof(UnityEngine.Object)) { }

        /// <summary>
        /// assigns a static field in the loading screen (field must inherit from UnityEngine.Object)
        /// (Unfortunately Attributes cannot be generic which is dumb)
        /// </summary>
        /// <param name="staticFieldName">name of the field on the item (just the simple name)</param>
        /// <param name="key">name of the resource in the Unity filesystem</param>
        /// <param name="resourceType">type of resource used for redundancy</param>
        public LoadResourceToPropertyAttribute(string staticFieldName, string key, Type resourceType)
        {
            this.type = resourceType;
            this.key = key;
            this.staticPropertyName = staticFieldName;
        }
    }

    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class LoadResourceToPropertyInheritedAttribute : Attribute
    {
        Type type;
        string key;
        string staticPropertyName;

        //folderPath ends with /
        public void Load(string folderPath, Type item)
        {
            //get the resource from the file system
            UnityEngine.Object o = Resources.Load(folderPath + key, type);

            //assign the resource to the static field
            var property = item.GetProperty(staticPropertyName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.SetProperty);

            //property is guarenteed to be static and set
            //however, if the resource type is wrong, then this line may very well throw an exception
            property.SetValue(null, o);
        }

        /// <summary>
        /// assigns a static field in the loading screen (field must inherit from UnityEngine.Object)
        /// (Unfortunately Attributes cannot be generic which is dumb)
        /// </summary>
        /// <param name="staticFieldName">name of the field on the item (just the simple name)</param>
        /// <param name="key">name of the resource in the Unity filesystem</param>
        public LoadResourceToPropertyInheritedAttribute(string staticFieldName, string key)
            : this(staticFieldName, key, typeof(UnityEngine.Object)) { }

        /// <summary>
        /// assigns a static field in the loading screen (field must inherit from UnityEngine.Object)
        /// (Unfortunately Attributes cannot be generic which is dumb)
        /// </summary>
        /// <param name="staticFieldName">name of the field on the item (just the simple name)</param>
        /// <param name="key">name of the resource in the Unity filesystem</param>
        /// <param name="resourceType">type of resource used for redundancy</param>
        public LoadResourceToPropertyInheritedAttribute(string staticFieldName, string key, Type resourceType)
        {
            this.type = resourceType;
            this.key = key;
            this.staticPropertyName = staticFieldName;
        }
    }
}

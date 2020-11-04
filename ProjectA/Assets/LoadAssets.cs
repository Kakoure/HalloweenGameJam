using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Items;

//loads assets from the Data folder using multithreading and reflection
public class LoadAssets : MonoBehaviour
{
    bool isEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        ThreadStart thread = () =>
        {
            Item.LoadItems();
            isEnd = true;
        };

        //create a new thread for loading items
        Thread loadItems = new Thread(thread);
        loadItems.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnd)
        {
            Debug.Log("Thread Ended");
            isEnd = false;
        }
    }
}

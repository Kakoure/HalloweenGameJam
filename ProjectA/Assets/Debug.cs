using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public bool paused { get; private set; }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(paused)
            {
                //unpause the game
                Time.timeScale = 1;

                paused = false;
            }
            else
            {
                //pause the game
                Time.timeScale = 0;

                paused = true;
            }
        }
    }
}

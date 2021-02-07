using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Events;

//rename this
//TODO: build a pause screen
public class PauseController : MonoBehaviour
{
    [System.Serializable]
    private class PauseToogleEvent : UnityEvent<bool> { }

    public static bool Paused { get; private set; }
    public static PauseController Insatance { get; private set; }

    [SerializeField]
    private PauseToogleEvent OnPauseToogle;

    // Start is called before the first frame update
    void Start()
    {
        if (Insatance != null) Debug.LogError("Multiple PauseControllers detected");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnPauseToogle.Invoke(Paused);

            if(Paused)
            {
                //unpause the game
                Time.timeScale = 1;

                Paused = false;
            }
            else
            {
                //pause the game
                Time.timeScale = 0;

                Paused = true;
            }
        }
    }
}

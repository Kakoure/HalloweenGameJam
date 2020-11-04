using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public static int option;

    public void SetOption(int o)
    {
        option = o;
    }

    public string GetScene()
    {
        switch (option)
        {
            case 1: return "Staff";
            case 2: return "Sword";
            case 3: return "Bow";
            case 4: return "Godmode";
            case 5: return "Unarmed";
            default: return "Staff";
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GetScene(), LoadSceneMode.Single);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

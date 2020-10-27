using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public static string sceneName = "Scene2";

    public void SetSceneName(string s)
    {
        sceneName = s;
    }

    public void SetWizard() { sceneName = "Wizard"; }
    public void SetWarrior() { sceneName = "Warrior"; }

    public void StartGame()
    {
        if (sceneName == null) return;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
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

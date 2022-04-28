using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject startButton;
    public GameObject quitButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("tutorialv2");
    }

    public void EndGame()
    {
        Debug.Log("Quitting game! Ignored in Editor");
        Application.Quit();
    }
}

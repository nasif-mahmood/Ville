using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpdateHUD : MonoBehaviour
{
    [Tooltip("Player GameObject with Player script")]
    public GameObject playerObject;
    [Tooltip("HUD Healthbar GameObject")]
    public GameObject hudHealthbar;
    [Tooltip("HUD Coin Counter GameObject")]
    public GameObject coinCounter;
    [Tooltip("Flag that a popup is active")]
    public bool isPopupActive = false;
    [Tooltip("Flag that pause menu is active")]
    public bool isPauseActive = false;
    [Tooltip("Flag for if in level transition/death menu")]
    public bool isTransitionActive = false;

    GameObject pauseMenu;   // Pause menu child GameObject
    GameObject deathMenu;   // Death menu child GameObject
    GameObject levelMenu;   // Level menu child GameObject
    GameObject endMenu;     // End menu child GameObject
    Image healthbar;    // Stores reference to HUD healthbar
    Player mainPlayer;  // Stores reference to player's Player script for HP/coins
    Text coinText;      // Stores reference to HUD text
    Image pauseBGImage; // Stores reference to pause menu's background
    float backgroundScaleRatio; // Ratio used to scale pauseBGImage to fill background
    Scene currentScene; // Current scene
    // Start is called before the first frame update
    void Start()
    {
        // Get references
        pauseMenu = transform.GetChild(4).gameObject;
        deathMenu = transform.GetChild(5).gameObject;
        levelMenu = transform.GetChild(6).gameObject;
        endMenu = transform.GetChild(7).gameObject;
        healthbar = hudHealthbar.GetComponent<Image>();
        mainPlayer = playerObject.GetComponent<Player>();
        coinText = coinCounter.GetComponent<Text>();
        pauseBGImage = pauseMenu.transform.GetChild(0).GetComponent<Image>();
        pauseBGImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        currentScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        // Reveals/hides pause menu
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            FlipPauseMenu();
        }
    }

    /*
    Updates HUD Visuals
    */
    public void UpdateVisuals()
    {
        healthbar.fillAmount = mainPlayer.currentHealth / mainPlayer.maxHealth;
        coinText.text = "X " + Player.currentCoins;
    }

    /*
    Reveals/Hides Pause Menu
    */
    public void FlipPauseMenu()
    {
        if (!isTransitionActive)
        {
            pauseMenu.active = !pauseMenu.active;
            isPauseActive = pauseMenu.active;
            if (!isPopupActive)
            {
                Time.timeScale = pauseMenu.active ? 0 : 1;
            }
        }
    }

    /*
     * Called on Player death from Player.cs
     */
    public void ShowDeathMenu()
    {
        deathMenu.active = true;
        isTransitionActive = true;
        Time.timeScale = 0;
        isPopupActive = true;
    }

    /*
     * Called when player chooses to restart level
     */
    public void PlayerRestart()
    {
        Time.timeScale = 1;
        currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    /*
     * Shows level transition popup
     */
    public void ShowLevelEndMenu()
    {
        levelMenu.active = true;
        isTransitionActive = true;
        Time.timeScale = 0;
        isPopupActive = true;
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        isTransitionActive = false;
        if (currentScene.name == "tutorialv2")
        {
            SceneManager.LoadScene("ForestLevel");
        }
        else if (currentScene.name == "ForestLevel")
        {
            SceneManager.LoadScene("CaveLevel");
        }
        else
        {
            Time.timeScale = 0;
            endMenu.active = true;
        }
    }

    /*
    Quits current Scene and loads MainMenu
    */
    public void ReturnToMainMenu()
    {
        // Reset everything just in-case
        Time.timeScale = 1;
        levelMenu.active = false;
        pauseMenu.active = false;
        isTransitionActive = false;
        SceneManager.LoadScene("MainMenu");
    }
}

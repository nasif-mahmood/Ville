using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHUD : MonoBehaviour
{
    [Tooltip("Player GameObject with Player script")]
    public GameObject playerObject;
    [Tooltip("HUD Healthbar GameObject")]
    public GameObject hudHealthbar;
    [Tooltip("HUD Coin Counter GameObject")]
    public GameObject coinCounter;
    [Tooltip("Pause Menu GameObject")]
    public GameObject pauseMenu;
    [Tooltip("Flag that a popup is active")]
    public bool isPopupActive = false;
    [Tooltip("Flag that pause menu is active")]
    public bool isPauseActive = false;

    Image healthbar;    // Stores reference to HUD healthbar
    Player mainPlayer;  // Stores reference to player's Player script for HP/coins
    Text coinText;      // Stores reference to HUD text
    Image pauseBGImage; // Stores reference to pause menu's background
    float backgroundScaleRatio; // Ratio used to scale pauseBGImage to fill background
    // Start is called before the first frame update
    void Start()
    {
        // Get references
        healthbar = hudHealthbar.GetComponent<Image>();
        mainPlayer = playerObject.GetComponent<Player>();
        coinText = coinCounter.GetComponent<Text>();
        pauseBGImage = pauseMenu.transform.GetChild(0).GetComponent<Image>();
        pauseBGImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
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
        coinText.text = "X " + mainPlayer.currentCoins;
    }

    /*
    Reveals/Hides Pause Menu
    */
    public void FlipPauseMenu()
    {
        pauseMenu.active = !pauseMenu.active;
        isPauseActive = pauseMenu.active;
        if (!isPopupActive)
        {
            Time.timeScale = pauseMenu.active ? 0 : 1;
        }
    }

    /*
    Quits current Scene and loads MainMenu
    */
    public void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}

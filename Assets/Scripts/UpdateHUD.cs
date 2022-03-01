using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHUD : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject hudHealthbar;
    public GameObject coinCounter;
    public GameObject pauseMenu;

    Image healthbar;
    Player mainPlayer;
    Text coinText;
    Image pauseBGImage;
    float backgroundScaleRatio;
    // Start is called before the first frame update
    void Start()
    {
        healthbar = hudHealthbar.GetComponent<Image>();
        mainPlayer = playerObject.GetComponent<Player>();
        coinText = coinCounter.GetComponent<Text>();
        pauseBGImage = pauseMenu.transform.GetChild(0).GetComponent<Image>();
        pauseBGImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        mainPlayer.TakeDamage(0.01f);
        mainPlayer.AddCoins(1);
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            FlipPauseMenu();
        }
    }

    public void UpdateVisuals()
    {
        healthbar.fillAmount = mainPlayer.currentHealth / mainPlayer.maxHealth;
        coinText.text = "X " + mainPlayer.currentCoins;
    }

    public void FlipPauseMenu()
    {
        pauseMenu.active = !pauseMenu.active;
        Time.timeScale = pauseMenu.active ? 0 : 1;
    }

    public void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}

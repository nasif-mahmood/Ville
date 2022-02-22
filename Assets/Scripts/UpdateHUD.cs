using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHUD : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject hudHealthbar;
    public GameObject coinCounter;

    Image healthbar;
    Player mainPlayer;
    Text coinText;
    // Start is called before the first frame update
    void Start()
    {
        healthbar = hudHealthbar.GetComponent<Image>();
        mainPlayer = playerObject.GetComponent<Player>();
        coinText = coinCounter.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        mainPlayer.TakeDamage(10);
        mainPlayer.AddCoins(1);
    }

    public void UpdateVisuals()
    {
        healthbar.fillAmount = mainPlayer.currentHealth / mainPlayer.maxHealth;
        coinText.text = "X " + mainPlayer.currentCoins;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [Tooltip("HUD to coordinate time pausing")]
    public GameObject hud;

    bool isShowingPopup = false;  // Flag for if popup on screen
    UpdateHUD hudScript;  // Used to coordinate time pausing with HUD script
    GameObject interactNotif;  // Checks if active = player nearby
    GameObject popupUI;  // Popup UI child
    // Start is called before the first frame update
    void Start()
    {
        popupUI = transform.GetChild(0).gameObject;
        interactNotif = transform.GetChild(1).gameObject;
        hudScript = hud.GetComponent<UpdateHUD>();
    }

    // Update is called once per frame
    void Update()
    {
        // If Player in range of popup, not pausing, and presses E then interact with popup
        if (interactNotif.active && !hudScript.isPauseActive && Input.GetKeyDown(KeyCode.E))
        {
            isShowingPopup = !isShowingPopup;
            hudScript.isPopupActive = isShowingPopup;
            Time.timeScale = isShowingPopup ? 0 : 1;
            popupUI.active = isShowingPopup;
        }
    }
}

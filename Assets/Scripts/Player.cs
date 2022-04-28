using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float maxHealth = 1000.0f;
    public float currentHealth = 1000.0f;
    public static int currentCoins;
    public GameObject hud;

    UpdateHUD hudUpdate;
    GameObject currentPopup;

    private Scene scene;
    
    // object to the enemy damage so that the Player script and EnemyDamage script can communicate
    EnemyDamage enemyDamageScript;

    // Start is called before the first frame update
    void Start()
    {
        hudUpdate = hud.GetComponent<UpdateHUD>();
        scene = SceneManager.GetActiveScene();
        Debug.Log("Name: " + scene.name);
        hudUpdate.UpdateVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth < 0)
        {
            OnDeath();
        }
        hudUpdate.UpdateVisuals();
    }

    public void GetHeal(float heal)
    {
        currentHealth = Mathf.Min(currentHealth + heal, maxHealth);
        hudUpdate.UpdateVisuals();
    }

    public void AddCoins(int coins)
    {
        currentCoins += coins;
        hudUpdate.UpdateVisuals();
    }

    void OnDeath()
    {
        // TODO: Add death UI screen and any on-death events
        Debug.Log("Player died! Oh no!");
        currentCoins = 0;
        // For testing purposes refill HP on death, delete later
        currentHealth = maxHealth;
        hudUpdate.UpdateVisuals();
        
        // reset the scene
        SceneManager.LoadScene(scene.name);
    }

    void endLevel()
    {
        Debug.Log("Entering Next Level");
        // fill hp back up
        currentHealth = maxHealth;
        hudUpdate.UpdateVisuals();
        
        // change to next scene
        if (scene.name == "ForestLevel")
        {
            SceneManager.LoadScene("CaveLevel");
        }
    }
    void OnTriggerEnter(Collider target)
    {
        // if the player touches the enemy and the enemy hasn't died yet, the player should take damage
        enemyDamageScript = target.GetComponent<EnemyDamage>();
        if (target.gameObject.tag.Equals("Enemy") && enemyDamageScript.enemyDeath() == false)
        {
            TakeDamage(50.0f);
        }
        else if (target.gameObject.tag.Equals("FinalEnemy") && enemyDamageScript.enemyDeath() == false)
        {
            TakeDamage(100.0f);
        }
        else if (target.gameObject.tag.Equals("Popup")) 
        {
            currentPopup = target.gameObject;
            currentPopup.transform.GetChild(1).gameObject.active = true;
        }
        else if (target.gameObject.tag.Equals("Coin"))
        {
            AddCoins(1);
        }
        else if (target.gameObject.tag.Equals("Heart"))
        {
            GetHeal(250.0f);
        }
        else if (target.gameObject.tag.Equals("GoldBar"))
        {
            AddCoins(10);
        }
        else if (target.gameObject.tag.Equals("Star"))
        {
            endLevel();
        }
        
        else if (target.gameObject.tag.Equals("Water"))
        {
            OnDeath();
        }
        
    }

    // void OnTriggerStay(Collider target)
    // {
    //     if(target.gameObject.tag.Equals("Enemy") && enemyDamageScript.enemyDeath() == false)
    //     {
    //         TakeDamage(10.0f);
    //     }
    // }

    void OnTriggerExit(Collider target)
    {
        if(target.gameObject.tag.Equals("Popup"))
        {
            currentPopup.transform.GetChild(1).gameObject.active = false;
            currentPopup = null;
        }
    }
}

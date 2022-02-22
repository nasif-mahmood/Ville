using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 1000.0f;
    public float currentHealth = 1000.0f;
    public int currentCoins;
    public GameObject hud;

    UpdateHUD hudUpdate;
    // Start is called before the first frame update
    void Start()
    {
        hudUpdate = hud.GetComponent<UpdateHUD>();
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
    }

    public void GetHeal(float heal)
    {
        currentHealth = Mathf.Max(currentHealth + heal, maxHealth);
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
    }
}

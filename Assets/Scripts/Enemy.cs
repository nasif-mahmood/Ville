using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject hudHealthbar;
    public float maxHealth = 100;
    public float currentHealth = 100;

    Image healthbar;
    // Start is called before the first frame update
    void Start()
    {
        healthbar = hudHealthbar.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // REMOVE - this is just for debugging
        TakeDamage(0.01f);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0.0f;
            OnDeath();
        }
        healthbar.fillAmount = currentHealth / maxHealth;
    }

    void OnDeath()
    {
        Debug.Log("Enemy died!");
        currentHealth = maxHealth;
    }
}

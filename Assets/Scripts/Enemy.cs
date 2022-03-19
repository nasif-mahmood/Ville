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
        // TODO: Implement enemy AI behavior
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

    /*
    Called when enemy dies
    */
    void OnDeath()
    {
        Debug.Log("Enemy died!");
        Destroy(this.gameObject);
    }

    /*
    Called when enemy is initially touched
    */
    void OnTriggerEnter(Collider target)
    {
        if(target.gameObject.tag.Equals("Player"))
        {
            TakeDamage(2.0f);
        }
    }

    /*
    Called when enemy is being touched
    */
    void OnTriggerStay(Collider target)
    {
        if(target.gameObject.tag.Equals("Player"))
        {
            TakeDamage(2.0f);
        }
    }
}

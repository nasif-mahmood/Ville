using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
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
        Enemy enemyScript = GameObject.FindObjectOfType(typeof(Enemy)) as Enemy;
        Debug.Log("Enemy died!");

        // call the function to set the death animation in the Enemy Script
        enemyScript.hasDiedAnim();
    }


    /*
    Called when enemy is initially touched
    */
    void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.tag.Equals("Player"))
        {
            TakeDamage(2.0f);
        }

        //if (target.gameObject.tag.Equals("Fire"))
        //{
        //    Debug.Log("Fireball hurt enemy");
        //    TakeDamage(10.0f);
        //}
    }

    /*
    Called when enemy is being touched
    */
    void OnTriggerStay(Collider target)
    {
        if (target.gameObject.tag.Equals("Player"))
        {
            TakeDamage(2.0f);
        }
    }
}

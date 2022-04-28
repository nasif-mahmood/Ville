using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    private Enemy enemyScript;
    public GameObject hudHealthbar;
    public float maxHealth = 100;
    public float currentHealth = 100;

    private bool hasDied = false;

    Image healthbar;

    public GameObject star;
    
    // Start is called before the first frame update
    void Start()
    {
        healthbar = hudHealthbar.GetComponent<Image>();
        enemyScript = GetComponentInParent<Enemy>();
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
        //Enemy enemyScript = GameObject.FindObjectOfType(typeof(Enemy)) as Enemy;
        Debug.Log("Enemy died!");

        hasDied = true;
        Vector3 deathPos = this.gameObject.transform.position;

        // call the function to set the death animation in the Enemy Script
        enemyScript.hasDiedAnim();
        if (this.gameObject.tag.Equals("FinalEnemy"))
        {
            Debug.Log("Final died!");
            GameObject goalStar = Instantiate(star, deathPos, Quaternion.identity);
        }
    }


    /*
    Called when enemy is initially touched
    */
    void OnTriggerEnter(Collider target)
    {
        //if (target.gameObject.tag.Equals("Player") && !hasDied)
        //{
        //    TakeDamage(2.0f);
        //}

        if (target.gameObject.tag.Equals("Fire") && !hasDied)
        {
            Debug.Log("Fireball hurt enemy");
            TakeDamage(20.0f);
        }

        if(target.gameObject.tag.Equals("Weapon") && !hasDied)
        {
            Debug.Log("Sword hurt enemy");
            TakeDamage(40.0f);
        }
    }

    /*
    Called when enemy is being touched
    */
    void OnTriggerStay(Collider target)
    {
        //if (target.gameObject.tag.Equals("Player") && !hasDied)
        //{
        //    TakeDamage(2.0f);
        //}
    }

    // to get the bool on if the enemy has died. Connected to the Player script
    // so the player knows whether the enemy is dead or not
    public bool enemyDeath()
    {
        return hasDied;
    }
}

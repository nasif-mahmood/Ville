using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class TempEnemyRemoveAfterMerge : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject hudHealthbar;
    public float maxHealth = 100;
    public float currentHealth = 100;

    Image healthbar;


    public LayerMask playerLayer;

    public NavMeshAgent agent;
    private GameObject player;
    private GameObject enemy;

    // When the player gets out of range, the enemy should return to its original location (if it can't within a
    // certain timeframe, transport the enemy back to its original location)
    private Vector3 originalLocation;
    private float moveStartTime;

    // if its in its initial location, let it move around a little in a small area and idle each time it reaches
    // a destination
    private Vector3 destinationPoint;
    private bool reachedDestination = false;
    public float walkRange;
    private float idleTime = 2.5f;


    // States
    public float sightRange;
    // range for when the enemy starts attacking
    public float attackRange;
    private bool playerInSightRange;
    private bool playerInAttackRange;



    // Start is called before the first frame update
    void Start()
    {
        healthbar = hudHealthbar.GetComponent<Image>();


        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        // question is if this is problematic if there is more than 1 enemy
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        originalLocation = enemy.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Implement enemy AI behavior

        // Check whether the enemy is within a certain range of the player
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        // state machine: enemy can either go back to its location, run to player, or attack player
        if (!playerInSightRange && !playerInAttackRange)
        {
            moveBack();
            moveAround();
        }

        //else if (playerInSightRange && !playerInAttackRange)
        //    moveToPlayer();
    }

    private void moveBack()
    {
        // attempt to move the enemy back to its initial location
        agent.SetDestination(originalLocation);
        moveStartTime = Time.time;

        // if its been 10 seconds and the enemy isn't in its original location yet, transport it
        // back to its initial location
        if (enemy.transform.position != originalLocation && Time.time - moveStartTime >= 10.0f)
        {
            enemy.transform.position = originalLocation;
        }

    }

    private void moveAround()
    {
        if (!reachedDestination)
        {
            float ZLocation = Random.Range(-walkRange, walkRange);
            float XLocation = Random.Range(-walkRange, walkRange);

            destinationPoint = new Vector3(transform.position.x + XLocation, transform.position.y, transform.position.z + ZLocation);

            reachedDestination = true;
        }
        else
        {
            agent.SetDestination(destinationPoint);
        }

        Vector3 distanceFromDestination = enemy.transform.position - destinationPoint;

        // if you are very close to your destination point, then you reached it
        if (distanceFromDestination.magnitude < 0.1f)
        {
            reachedDestination = true;

            // idle for a bit before choosing a new destination
            // probably buggy/may not work, need to test this
            StartCoroutine(idle());
        }

    }

    private IEnumerator idle()
    {
        yield return new WaitForSeconds(idleTime);
        // set the animation for idling here
    }

    private void moveToPlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    private void attackPlayer()
    {
        // have the enemy stop once it reaches attack distance to the player
        agent.SetDestination(transform.position);

        transform.LookAt(player.transform);
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
        if (target.gameObject.tag.Equals("Player"))
        {
            TakeDamage(2.0f);
        }
        Debug.Log(target.gameObject.tag);
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

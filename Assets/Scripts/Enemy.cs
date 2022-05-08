using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    public struct DeathInfo
    {
        public DeathInfo(Vector3 pos, bool tag)
        {
            Pos = pos;
            Tag = tag;
        }

        public Vector3 Pos { get; }
        public bool Tag { get; }

        //public override string ToString() => $"({X}, {Y})";
    }
    
    //public GameObject hudHealthbar;
    //public float maxHealth = 100;
    //public float currentHealth = 100;

    //Image healthbar;



    // Enemy AI variables
    public LayerMask playerLayer;
    public LayerMask floorLayer;

    public NavMeshAgent agent;
    private GameObject player;
    //public GameObject enemy;
    private Transform enemy;
    private Animator enemy_anim;


    // When the player gets out of range, the enemy should return to its original location (if it can't within a
    // certain timeframe, transport the enemy back to its original location)
    private Vector3 originalLocation;

    // if its in its initial location, let it move around a little in a small area and idle each time it reaches
    // a destination
    private Vector3 destinationPoint;
    private bool reachedDestination = false;
    public float walkRange;

    // decrements each time it makes a move, resets once it reaches 0
    // ONLY WORKS IF YOU SET IT TO 1, acts like a semaphore
    private int numMovesLeft = 1;

    // determines whether the enemy has just finished attacking
    private bool isAttacking = false;


    // States
    public float sightRange;
    // range for when the enemy starts attacking
    public float attackRange;
    private bool playerInSightRange;
    private bool playerInAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        // healthbar = hudHealthbar.GetComponent<Image>();

        enemy = this.transform;

        // Enemy AI stuff
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        enemy_anim = GetComponent<Animator>();

        originalLocation = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check whether the enemy is within a certain range of the player
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        // state machine: enemy can either go back to its location, run to player, or attack player
        // only keep going if the enemy is still alive
        if(enemy_anim.GetBool("hasDied") == false)
        {
            if (!playerInSightRange && !playerInAttackRange)
            {
                moveAround();
            }
            if (playerInSightRange && !playerInAttackRange)
            {
                //Debug.Log("priorFramePosition: ");
                //Debug.Log(priorFramePosition);
                //Debug.Log(enemy.transform.position);
                //if(Vector3.Distance(enemy.transform.position, priorFramePosition) > 0.01f)
                    moveToPlayer();
            }
            if (playerInSightRange && playerInAttackRange)
            {
                attackPlayer();
            }
        }
        
    }

    private void moveAround()
    {
        // if enemy was moving before, set the animation back to not moving now
        enemy_anim.SetBool("isMoving", false);
        // determines the distance from the enemy to its destination
        Vector3 distanceFromDestination;

        NavMeshHit navHit;

        // only allows one thread in at a time
        if (numMovesLeft > 0)
        {
            if (!reachedDestination)
             {

                //Debug.Log("pick a new destination");

                // pick random x and z coordinates that are within the original location's range
                float newX = originalLocation.x + Random.Range(-walkRange, walkRange);
                float newZ = originalLocation.z + Random.Range(-walkRange, walkRange);

                // set this new point as a point to move to
                destinationPoint = new Vector3(newX, transform.position.y, newZ);

                // set this position relative to the NavMeshMap so the enemy won't try to move
                // somewhere outside of the NavMesh
                NavMesh.SamplePosition(destinationPoint, out navHit, walkRange, -1);

                // debating whether to remove this if statement
                // would be to check if the destination is within the map
                if (Physics.Raycast(destinationPoint, -transform.up, 2f, floorLayer))
                    reachedDestination = true;

                // set the destination considering the navMesh to the destinationPoint
                destinationPoint = navHit.position;
            }
            else
            {
                // move to the point that was generated
                enemy_anim.SetBool("isMoving", true);
                agent.SetDestination(destinationPoint);
            }

            distanceFromDestination = this.transform.position - destinationPoint;

            // if you are very close to your destination point, then you reached it
            if (distanceFromDestination.magnitude < 0.1f)
            {
                reachedDestination = false;

                // numMovesLeft is basically a semaphore, only allows 1 thread to enter
                // at a time
                numMovesLeft--;

                // idle for a bit before choosing a new destination
                StartCoroutine(idle());
            }
        }
    }

    private IEnumerator idle()
    {
        enemy_anim.SetBool("isMoving", false);
        yield return new WaitForSeconds(2);

        // incrementing, allowing another move
        numMovesLeft++;
    }

    private void moveToPlayer()
    {
        enemy_anim.SetBool("isMoving", true);
        agent.SetDestination(player.transform.position);
    }

    private void attackPlayer()
    {
        // have the enemy stop once it reaches attack distance to the player
        agent.SetDestination(transform.position);

        transform.LookAt(player.transform);

        if(!isAttacking)
        {
            // set animator to an attack trigger here
            enemy_anim.SetTrigger("isAttacking");
            StartCoroutine(initializeAttack());
            StartCoroutine(resetAttack());
        }

    }

    IEnumerator initializeAttack()
    {
        // give the trigger a moment to set
        yield return new WaitForSeconds(0.1f);
        isAttacking = true;
    }

    IEnumerator resetAttack()
    {
        // wait for the attack time to finish through
        yield return new WaitForSeconds(4.0f);
        isAttacking = false;
    }

    // to play the animator of the enemy once it dies
    public DeathInfo hasDiedAnim()
    {
        enemy_anim = GetComponentInParent<Animator>();
        enemy_anim.SetBool("hasDied", true);
        bool isFinal = this.gameObject.tag.Equals("FinalEnemy");
        Vector3 deathPos = this.gameObject.transform.position;
        var d1 = new DeathInfo(deathPos, isFinal);

        Destroy(enemy.gameObject, 3f);
        return d1;
    }

    //public void TakeDamage(float damage)
    //{
    //    currentHealth -= damage;
    //    if (currentHealth <= 0)
    //    {
    //        currentHealth = 0.0f;
    //        OnDeath();
    //    }
    //    healthbar.fillAmount = currentHealth / maxHealth;
    //}

    ///*
    //Called when enemy dies

    //void OnDeath()
    //{
    //    Debug.Log("Enemy died!");
    //    Destroy(this.gameObject);
    //}

    ///*
    //Called when enemy is initially touched
    //*/
    //void OnTriggerEnter(Collider target)
    //{
    //    if(target.gameObject.tag.Equals("Player"))
    //    {
    //        TakeDamage(2.0f);
    //    }
    //}

    ///*
    //Called when enemy is being touched
    //*/
    //void OnTriggerStay(Collider target)
    //{
    //    if(target.gameObject.tag.Equals("Player"))
    //    {
    //        TakeDamage(2.0f);
    //    }
    //}
}

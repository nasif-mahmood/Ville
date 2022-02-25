using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMovement : MonoBehaviour
{
    Vector3 playerDirection;
    
    // access to the character and its collider
    public Rigidbody character;
    BoxCollider boxCollider;
    //public Transform cam;

    // next 2 variables for collision check with the floor
    public Transform feetTransform;
    public LayerMask FloorMask;

    // holds whether the player is on the ground or not
    private bool isGrounded;

    // holds the number of times a player has jumped in a row (should only allow 2 times for double jump)
    private bool canDoubleJump = false;

    // detects whether player is crawling or not
    private bool isCrawling = false;

    public float speed = 5f;
    public float jumpForce = 3f;
    public float rotationSpeed = 75.0f;
    
    // use this for initialization
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void FixedUpdate()
    {
        // get the movement axes
        float horizontal = Input.GetAxis("Horizontal") * speed;
        float vertical = Input.GetAxis("Vertical") * speed;

        // so it works independent of framerate
        horizontal = horizontal * Time.deltaTime;
        vertical = vertical * Time.deltaTime;

        // allows for movement in x,y, and z directions
        // normalized so that we don't go faster when we go diagonal (hitting 2 keys)
        playerDirection = new Vector3(horizontal, 0f, vertical);

        MovePlayer(horizontal, vertical);
    }

    private void MovePlayer(float horizontal, float vertical)
    {
        // Moving
        transform.Translate(transform.TransformDirection(-horizontal, 0f, vertical));

        // move around using the moveVector coordinates, let gravity be controlled by the rigidbody attributes
        //character.velocity = new Vector3(moveVector.x, character.velocity.y, moveVector.z);

        // Turning
        // If player is moving, allow for rotation to occur
        if(playerDirection != Vector3.zero)
        {
            // the player's direction should always follow the blue axis of the transform
            transform.forward = playerDirection;
            transform.rotation = Quaternion.LookRotation(playerDirection);
        }

    }
    // Update is called once per frame (put jump in here to make it more responsive)
    void Update()
    {
        UpdateGroundedStatus();

        //jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            character.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canDoubleJump = true;
        }
        else if(canDoubleJump && Input.GetButtonDown("Jump"))
        {
            canDoubleJump = false;
            character.AddForce(Vector3.up * (jumpForce + 3), ForceMode.Impulse);
        }

        // Crawling
        //Crawl();

    }

    // My attempt at a crawl (very buggy)
    /*void Crawl()
    {
        if (Input.GetButton("Crouch"))
        {
            isCrawling = !isCrawling;
            boxCollider.size = new Vector3(1f, 1.282183f, 0.8f);
            transform.rotation = Quaternion.LookRotation(new Vector3(playerDirection.x, -90f, playerDirection.z));
        }
        else if (isCrawling == false)
        {
            boxCollider.size = new Vector3(1f, 1.282183f, 1f);
            transform.rotation = Quaternion.LookRotation(playerDirection);
        }
           
    }*/

    void UpdateGroundedStatus()
    {
        // check if the player is on the ground
        isGrounded = Physics.CheckSphere(feetTransform.position, 0.1f, FloorMask);
           
    }
}



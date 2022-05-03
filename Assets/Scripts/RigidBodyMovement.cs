using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMovement : MonoBehaviour
{
    Vector3 playerDirection;
    
    // access to the character
    public Rigidbody character;

    // for dog animator
    private Animator d_anim;

    // To get access to the main camera
    public Transform cam;

    // next 2 variables for collision check with the floor
    public Transform feetTransform;
    public LayerMask FloorMask;

    // holds whether the player is on the ground or not
    private bool isGrounded;

    // holds the number of times a player has jumped in a row (should only allow 2 times for double jump)
    private bool canDoubleJump = false;

    // stores the current gravity of the player, used for double jump calculations
    private float currentGravity;

    public float speed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 7f;
    public float rotationSpeed = 75.0f;

    // Another variable that holds the speed so you can switch back to default after running
    private float defaultSpeed;

    // use this for initialization
    private void Start()
    {
        defaultSpeed = speed;
        d_anim = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        // get the movement axes
        float horizontal = Input.GetAxis("Horizontal") * speed;
        float vertical = Input.GetAxis("Vertical") * speed;

        // allows for movement in x,y, and z directions
        // normalized so that we don't go faster when we go diagonal (hitting 2 keys)
        //playerDirection = new Vector3(horizontal, 0f, vertical).normalized;

        MovePlayer(horizontal, vertical);
    }

    private void MovePlayer(float horizontal, float vertical)
    {
        // Moving

        // get the vectors of the forward and right directions of the main camera
        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;

        // zero out the y directions of these vectors since our main camera will be at some y angle
        // and we don't want that y angle to affect the character movements
        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        // determine direction of the player relative to the camera
        // note that if we want direction non-relative to camera, do the following:
        // playerDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // its normalized so that pushing 2 keys won't result in faster movement
        playerDirection = ((camF * vertical + camR * horizontal) * Time.deltaTime).normalized;

        // move the player relative to camera
        transform.position += (camF * vertical + camR * horizontal)*Time.deltaTime;


        // Turning
        // If player is moving, allow for rotation to occur and set the move animation
        if(playerDirection != Vector3.zero)
        {
            d_anim.SetBool("isMoving", true);
            transform.rotation = Quaternion.LookRotation(playerDirection);
        }

        // else, keep it at the idle animation
        else
        {
            d_anim.SetBool("isMoving", false);
        }

        

        // Run Button
        if (Input.GetKey("left shift"))
        {
            speed = runSpeed;
        }
        else
        {
            // if button is not pushed, the speed should be the default
            speed = defaultSpeed;
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

            // store the current gravity of the character
            currentGravity = character.velocity.y;

            canDoubleJump = false;

            // if the character is falling and not close to ground, make the jumpForce greater
            if (currentGravity < -0.1f)
            {
                Debug.Log("falling, not close to ground");

                // -currentGravity nullifies the downward force, adding jumpforce gives character
                // upward force
                character.AddForce(Vector3.up * -currentGravity + new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }

            // if the player is close to the ground and is not falling, make the jumpForce smaller
            else if (Physics.CheckSphere(feetTransform.position, 0.8f, FloorMask)
                     && currentGravity > 0.1f)
            {
                Debug.Log("close to ground, not falling");
                character.AddForce(Vector3.up * (jumpForce - 4), ForceMode.Impulse);
            }

            // else, player is jumping while going up and is not close to ground
            else
            {
                character.AddForce(Vector3.up * (jumpForce - 3), ForceMode.Impulse);
            }

        }
    }

    void UpdateGroundedStatus()
    {
        // check if the player is on the ground
        isGrounded = Physics.CheckSphere(feetTransform.position, 0.1f, FloorMask);
           
    }

    // Gets the animator for the dog
    public Animator getDogAnim()
    {
        return d_anim;
    }
}



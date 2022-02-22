using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    private float fallSpeed = 15f;
    public float jumpHeight = 3f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // variables for gravity and jumping
    private float gravity = 9.8f;
    private float verticalVelocity = 0;
    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        // get the value of the virtual axis. Values will either be
        // -1, 0, or 1 in movement, no in betweens
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical"); 
        
        // allows for movement in x,y, and z directions
        // normalized so that we don't go faster when we go diagonal (hitting 2 keys)
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            // player rotation
            // get the target angle of our movement in degrees
            // add by cam.eulerAngles.y to make sure player points the right way relative to the camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // smooth the rotation before it occurs (transform.eulerAngles.y is our current angle)
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // rotate on the y
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Allows player movement to adapt to wherever the camera is pointing
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // deltaTime allows the movement to be independent of frame rate
            // move the character
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Gravity Code
        // if its on the ground, vertical velocity is 0
        // if(controller.isGrounded)
        if (IsGrounded())
        {
            verticalVelocity = 0f;
        }
        // if on the ground and space is pushed, verticalVelocity changes
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = jumpHeight;
        }
        // to determine the player's y position if they fall or jump
        verticalVelocity = verticalVelocity + direction.y - (gravity * Time.deltaTime);
        velocity.y = verticalVelocity;

        controller.Move(velocity * fallSpeed * Time.deltaTime);


    }
    private bool IsGrounded()
    {
        float extraHeight = 0.1f;

        // finds the middle of the box collider
        //Vector3 colliderLocation = capsuleCollider.bounds.center;
        Color rayColor;
        bool hasHit = Physics.Raycast(controller.bounds.center, Vector3.down, controller.bounds.extents.y + extraHeight);

        if (hasHit)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(controller.bounds.center, Vector3.down * (controller.bounds.extents.y + extraHeight), rayColor);
        return hasHit;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMovement : MonoBehaviour
{
    Vector3 playerDirection;
    
    public Rigidbody character;
    //public Transform cam;

    public float speed = 5f;
    public float jumpForce = 3f;
    public float rotationSpeed = 75.0f;


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
        if(playerDirection != Vector3.zero)
        {
            transform.forward = playerDirection;
            transform.rotation = Quaternion.LookRotation(playerDirection);

        }
    }
    // Update is called once per frame (put jump in here to make it more responsive)
    void Update()
    {
        //jumping
        if (Input.GetButtonDown("Jump"))
        {
            character.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }
}



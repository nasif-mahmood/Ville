using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class FollowPlayer : MonoBehaviour
{
    /*public GameObject followObject;
    
    Transform cameraPos;
    Vector3 relativePos;*/

    public Transform followObject;
    public float smoothing = 5f;

    Vector3 relativePos;

    // Start is called before the first frame update
    void Start()
    {
        /* Assert.IsNotNull(followObject);
         cameraPos = GetComponent<Transform>();
         relativePos = cameraPos.position - followObject.transform.position;
        */
        // calculate initial offset
        relativePos = transform.position - followObject.position;
    }

    // Update is called once per frame
    void Update()
    {
        //cameraPos.position = followObject.transform.position + relativePos;

        // create position camera is aiming for
        Vector3 targetCameraPos = followObject.position + relativePos;

        // smooth movement between camera's current position and game object
        transform.position = Vector3.Lerp(transform.position, targetCameraPos, smoothing * Time.deltaTime);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class FollowPlayer : MonoBehaviour
{
    public GameObject followObject;
    
    Transform cameraPos;
    Vector3 relativePos;
    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(followObject);
        cameraPos = GetComponent<Transform>();
        relativePos = cameraPos.position - followObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        cameraPos.position = followObject.transform.position + relativePos;
    }
}

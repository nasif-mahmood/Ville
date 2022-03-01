using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
   Camera mainCamera;
   Transform currentPos;
    // Start is called before the first frame update
    void Start()
    {
      mainCamera = Camera.main;
      currentPos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
      currentPos.rotation = mainCamera.transform.rotation;
    }
}

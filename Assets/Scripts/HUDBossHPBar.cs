using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDBossHPBar : MonoBehaviour
{
    public GameObject bossHP;
    // Start is called before the first frame update
    void Start()
    {
        bossHP.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            bossHP.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            bossHP.SetActive(false);
        }
    }
}

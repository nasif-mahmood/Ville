using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDBossHPBar : MonoBehaviour
{
    public GameObject boss;
    public int hpChildIndex = 3;
    private GameObject bossHP;
    // Start is called before the first frame update
    void Start()
    {
        bossHP = boss.transform.GetChild(hpChildIndex).gameObject;
        bossHP.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        bossHP.active = true;
    }

    private void OnTriggerExit(Collider other)
    {
        bossHP.active = false;
    }
}

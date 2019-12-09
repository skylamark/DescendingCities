using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVBTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AVB")
        {
            other.GetComponent<AntiViewBlocker>().Disable();
            //Debug.Log("MAke building Dissapear");
        }

    }


    void OnTriggerExit(Collider other)
    {
        if (other.tag == "AVB")
        {
            other.GetComponent<AntiViewBlocker>().Enable();
            //Debug.Log("MAke building Appear");
        }
    }
}

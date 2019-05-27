using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public static Collider interactedPerson;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
      if(other.tag == "Interactable")
        {
            interactedPerson = other;
            Debug.Log(interactedPerson.name);
        }
    }

    public void OnTriggerExit()
    {
       

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour
{
    public float speed = 5;
    public GameObject playerCharacterObject;

    void Start()
    {
        
    }
    
    void Update()
    {
        Vector3 direction = MoveVector();
        transform.LookAt(playerCharacterObject.transform.position + direction);
        if (direction != Vector3.zero)
        {


            playerCharacterObject.transform.position += playerCharacterObject.transform.forward * speed * Time.deltaTime * Vector3.Magnitude(direction);
        }

    }

    private Vector3 MoveVector()
    {
        Vector3 direction = Vector3.zero;


        if (Input.GetKey("w"))
        {
            direction.z = 1;
        }
        if (Input.GetKey("s"))
        {
            direction.z = -1;
        }
        if (Input.GetKey("d"))
        {
            direction.x = 1;
        }
        if (Input.GetKey("a"))
        {
            direction.x = -1;
        }

        return direction;
    }
   
}

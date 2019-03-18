using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectional : MonoBehaviour {
    public GameObject playerCharacterObject;
    public int speed = 0;
    private float _speed;
    public bool died = false;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (died) return;
         Move(); 
    }

    private void Move()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction.z = 1;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            direction.z = -1;
        }


        if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;
        }

        transform.LookAt(this.transform.position + direction);
        if (direction != Vector3.zero)
        {
            this.transform.position += this.transform.forward * speed*Time.deltaTime;
        }
    }
}

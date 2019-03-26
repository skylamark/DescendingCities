using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectional : MonoBehaviour {
    public GameObject playerCharacterObject;
    public float speed = 3.0f;
    private float _speed;
    public bool died = false;
    public JoystickController joystick;

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
        /* Direction now comes from Joystick
         * 
         * Vector3 direction = Vector3.zero;

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
        }*/

        Vector3 direction = MoveVector();
        transform.LookAt(this.transform.position + direction);
        if (direction != Vector3.zero)
        {
            this.transform.position += this.transform.forward * speed*Time.deltaTime;
        }
    }

    private Vector3 MoveVector()
    {
        Vector3 direction = new Vector3(joystick.Horizantal(), 0, joystick.Vertical());
        if (direction.magnitude > 1)
            direction.Normalize();

        return direction;
    }
}

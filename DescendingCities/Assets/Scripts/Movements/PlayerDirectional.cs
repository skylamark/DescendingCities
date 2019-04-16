using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDirectional : MonoBehaviour {
    public GameObject playerCharacterObject;
    public float speed = 3.0f;
    private float _speed;
    public bool died = false;
    public JoystickController joystick;

  
    

    // Use this for initialization
    
    void Start () {
        //Load();
	}
	
	// Update is called once per frame
	void Update () {
        if (died) return;
         Move();
        if (Time.time > 5)
        {
            PlayerManagement.giveTask();
           
        }
    }

    private void OnApplicationQuit()
    {
        Save();
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

    public void Save()
    {
        SaveLoad.SaveGame(this);
    }
    public void Load()
    {
       GameData gameData = SaveLoad.LoadGame();
        if (gameData == null)
        {
            return;
        }
        else
        {
            speed = gameData.Player.Speed;
            died = gameData.Player.Died;
            Vector3 pos = new Vector3(gameData.Player.PositionX,
                                      gameData.Player.PositionY,
                                      gameData.Player.PositionZ);
            this.transform.position = pos;
           
        }

    }
  
}

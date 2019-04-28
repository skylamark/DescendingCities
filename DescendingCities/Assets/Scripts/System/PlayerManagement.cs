using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public static Player player = new Player();
    public Level CurrentLevel;
    public static Level currLevel;

    void Start()
    {
        currLevel = CurrentLevel;
        player.CurrTask = null;
       
       
       

    }

    private void Update()
    {
        if (currLevel.isComplete())
        {
            Debug.Log("Cong. you completed the level");
        }
    }

  
   
}

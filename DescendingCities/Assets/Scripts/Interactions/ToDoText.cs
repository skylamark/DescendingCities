using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToDoText : MonoBehaviour
{
    Player player;
    Text toDoText;
    RawImage toDoUI;

    void Start()
    {
        player = PlayerManagement.player;
        toDoText = GetComponentInChildren<Text>();
        toDoUI = GetComponent<RawImage>();
        toDoUI.enabled = false;
        toDoText.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isAvailable())
        {
            toDoUI.enabled = true;
            toDoText.enabled = true;
         //   toDoText.text = "TO-DO: \nGo to " + player.CurrTask.Location;
        }
        else
        {
            toDoUI.enabled = false;
            toDoText.enabled = false;
        }
    }
}

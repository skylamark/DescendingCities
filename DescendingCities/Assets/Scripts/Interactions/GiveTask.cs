using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiveTask : MonoBehaviour
{
    private Canvas canvas;
    public Text text;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
      


    }
    void Update()
    {
        if (PlayerManagement.player.isBusy())
        {
            canvas.enabled = true;
            text.text = PlayerManagement.player.CurrTask.Owner.Name + " says " +
            PlayerManagement.player.CurrTask.Situation + " in the " +
            PlayerManagement.player.CurrTask.Location;

        }
        else
        {

            canvas.enabled = false;
        }
    }

    public void Press()
    {
        PlayerManagement.player.CurrTask.ActivityComplete = true;
        PlayerManagement.player.CurrTask.carryOwner();
        PlayerManagement.player.Solutions.Add(PlayerManagement.player.CurrTask.RewardOfTask);
        PlayerManagement.player.CurrTask = null;
        if (PlayerManagement.player.CurrLevel.isComplete())
            Debug.Log("Level is completed");
    }
   

}

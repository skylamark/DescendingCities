using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptDecline : MonoBehaviour
{
    public static DialogueSystem dialogSystem;
    public static Task task;
   

    public static void onAccept()
    {
        if (PlayerManagement.player.isAvailable())
        {
            PlayerManagement.player.CurrTask = task;
            dialogSystem.enabled = false;

            Debug.Log(PlayerManagement.player.CurrTask.situation);
        }
    }
    public static void onDecline()
    {
        
        dialogSystem.enabled = false;
    }
}

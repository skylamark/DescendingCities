using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEditor;

[System.Serializable]
public class NPCForInfo : MonoBehaviour
{

    public Transform ChatBackGround;
    public Transform NPCCharacter;

    private DialogueSystem dialogueSystem;

    public string[] sentences;
    public string Name;

   

   

    void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
        

    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        this.gameObject.GetComponent<NPCForInfo>().enabled = true;
        FindObjectOfType<DialogueSystem>().EnterRangeOfNPC();
        if ((other.gameObject.tag == "Player"))
        {
            this.gameObject.GetComponent<NPCForInfo>().enabled = true;
            dialogueSystem.Names = Name;
            dialogueSystem.dialogueLines = sentences;
            FindObjectOfType<DialogueSystem>().NPCName();
        }
    }

    public void OnTriggerExit()
    {
        FindObjectOfType<DialogueSystem>().OutOfRange();
        this.gameObject.GetComponent<NPCForInfo>().enabled = false;
        if(!PlayerManagement.player.isAvailable() && (PlayerManagement.player.CurrTask.TaskID == 0
            || PlayerManagement.player.CurrTask.TaskID == 1
            || PlayerManagement.player.CurrTask.TaskID == 2
            || PlayerManagement.player.CurrTask.TaskID == 3))
        {
  
            PlayerManagement.player.CurrTask.Status = 2;
        }
           
    }

  

}


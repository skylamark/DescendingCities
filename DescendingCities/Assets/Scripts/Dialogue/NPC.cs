using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEditor;


public class NPC : MonoBehaviour
{

    public Transform ChatBackGround;
    public Transform NPCCharacter;

    private DialogueSystem dialogueSystem;

    private Owner owner;
    
    void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
        owner = GetComponent<Owner>();
       
    }
  
    void Update()
    {

            
    }

    public void OnTriggerEnter(Collider other)
    {
        this.gameObject.GetComponent<NPC>().enabled = true;
        FindObjectOfType<DialogueSystem>().EnterRangeOfNPC();
        if ((other.gameObject.tag == "Player"))
        {
            this.gameObject.GetComponent<NPC>().enabled = true;
            dialogueSystem.Names = owner.Name;
            dialogueSystem.dialogueLines = Sentence();
            FindObjectOfType<DialogueSystem>().NPCName();
        }

        if (owner.Task.Status == 0 && PlayerManagement.player.isAvailable())
        {
            dialogueSystem.currentOwner = owner;
            Debug.Log(owner.Name);

        }
    }

    public void OnTriggerExit()
    {
        FindObjectOfType<DialogueSystem>().OutOfRange();
        this.gameObject.GetComponent<NPC>().enabled = false;
    }

 
 
    public string[] splitToSentences(string s)
    {
        return s.Split('&');

    }

    private string[] Sentence()
    {
        Task task = owner.Task;
        //Task never given yet and player don't have another task
        if (owner.Task.Status == 0 && PlayerManagement.player.isAvailable()) 
        {
            
           // owner.task.status = 1;
           // PlayerManagement.player.CurrTask = owner.task;
            return splitToSentences(task.InitialConversation);


        }
        //Task never given yet but player is busy with another task
        else if (task.Status == 0 && !PlayerManagement.player.isAvailable())
        {
            return splitToSentences(task.TellPlayerHeIsBusy);
        }

        //Task is given but not completed yet
        else if (task.Status == 1)
        {
            return splitToSentences(task.WaitingForSolution);
        }
        //Task is given and completed but player didn't back to NPC yet.
        else if (task.Status == 2)
        {
           
            owner.task.Status = 3;
            PlayerManagement.player.CurrTask = null;
            JoinVVE(owner);
            return splitToSentences(task.CompletionMessage); 
        }
        //Task is alreard completed or this task is not in this level
        else if (task.Status == 3)
        {
            return splitToSentences(task.GenericResponse);
        }
        else
        {
            return null;
        }

    }

    public void JoinVVE(Owner o)
    {
        PlayerManagement.currLevel.Vve.Add(o);
        PlayerManagement.currLevel.Individuals.Remove(o);
        Debug.Log(o.Name + " joined vve");
    }
}


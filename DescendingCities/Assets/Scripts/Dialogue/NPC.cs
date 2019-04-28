using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEditor;

[System.Serializable]
public class NPC : MonoBehaviour
{

    public Transform ChatBackGround;
    public Transform NPCCharacter;

    private DialogueSystem dialogueSystem;

    private Owner owner;

    public string Name;

    private const int SIZE = 4;

    [TextArea(5, 10)]
    public string[] sentences = new string[SIZE];

    void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
        owner = GetComponent<Owner>();
    }
  
    void Update()
    {
      
    }

    public void OnTriggerStay(Collider other)
    {
        this.gameObject.GetComponent<NPC>().enabled = true;
        FindObjectOfType<DialogueSystem>().EnterRangeOfNPC();
        if ((other.gameObject.tag == "Player") && Input.GetMouseButtonDown(0))
        {
            this.gameObject.GetComponent<NPC>().enabled = true;
            dialogueSystem.Names = Name;
            dialogueSystem.dialogueLines = Sentence();
            FindObjectOfType<DialogueSystem>().NPCName();
        }
    }

    public void OnTriggerExit()
    {
        FindObjectOfType<DialogueSystem>().OutOfRange();
        this.gameObject.GetComponent<NPC>().enabled = false;
    }

    void OnValidate()
    {
        if (sentences.Length != SIZE)
        {
            Debug.LogWarning("Don't change the array size!");
            Array.Resize(ref sentences, SIZE);
        }
    }

 
    public string[] splitToSentences(string s)
    {
        return s.Split('.');

    }

    private string[] Sentence()
    {
        Task task = owner.Task;

        if (owner.Task.Status == 0 && PlayerManagement.player.isAvailable())
        {

            return splitToSentences(task.InitialConversation);


        }
        else if (task.Status == 0 && !PlayerManagement.player.isAvailable())
        {
            return splitToSentences(task.TellPlayerHeIsBusy);
        }
        else if (task.Status == 2)
        {
            return splitToSentences(task.CompletionMessage); 
        }
        else if(task.Status == 3)
        {
            return splitToSentences(task.GenericResponse);
        }
        else
        {
            return null;
        }

    }
}


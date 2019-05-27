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

    public string sentence;
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
        dialogueSystem.isOwner = false;
        this.gameObject.GetComponent<NPCForInfo>().enabled = true;
        FindObjectOfType<DialogueSystem>().EnterRangeOfNPC();
        if ((other.gameObject.tag == "Player"))
        {
            this.gameObject.GetComponent<NPCForInfo>().enabled = true;
            dialogueSystem.Names = Name;
            dialogueSystem.dialogueLines = splitToSentences(sentence);
            FindObjectOfType<DialogueSystem>().NPCName();
        }
    }

    public void OnTriggerExit()
    {
        FindObjectOfType<DialogueSystem>().OutOfRange();
        this.gameObject.GetComponent<NPCForInfo>().enabled = false;
      
           
    }

    public string[] splitToSentences(string s)
    {
        return s.Split('&');



    }

    }


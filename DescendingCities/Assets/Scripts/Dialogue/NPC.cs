using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

[System.Serializable]
public class NPC : MonoBehaviour {

    public Transform ChatBackGround;
    public Transform NPCCharacter;

    public DialogueSystem dialogueSystem;

    private Owner owner;

    public string Name;

   

    [TextArea(5, 10)]
    public string sentence;

    void Start () {

        dialogueSystem.OutOfRange();
        this.gameObject.GetComponent<NPC>().enabled = false;

        owner = GetComponent<Owner>();
       

    }
	
	void Update () {
        //  Vector3 Pos = Camera.main.WorldToScreenPoint(NPCCharacter.position);
         // Pos.y += 175;
          ChatBackGround.position = new Vector3(5, 1, 7);

       
    }

    public void OnTriggerStay(Collider other)
    {
        
        this.gameObject.GetComponent<NPC>().enabled = false;
        dialogueSystem.EnterRangeOfNPC();
        if (other.gameObject.tag == "Player")
        {
           
            this.gameObject.GetComponent<NPC>().enabled = true;
            dialogueSystem.Names = owner.Name;
            
            dialogueSystem.Line = Sentence();
            dialogueSystem.NPCName();
        }
    }

    public void OnTriggerExit()
    {
        Debug.Log("Exit");
        dialogueSystem.OutOfRange();
        this.gameObject.GetComponent<NPC>().enabled = false;
        
    }

    private string Sentence()
    {
        Task task = owner.Task;

        if (owner.Task.Status == 0 && PlayerManagement.player.isAvailable())
        {

            
            StartCoroutine(WaitToFinishDialog(task));
            return "situation: " + task.Situation
                + "\nlocation: " + task.location;

            
        }
        else if(task.Status == 1 && !PlayerManagement.player.isAvailable())
        {
            return "I am looking forward";
        }
        else if(task.Status == 0 && !PlayerManagement.player.isAvailable())
        {
            return "You seem very busy";
        }
        else if(task.Status == 2)
        {
            return "Thanks for your help last time";
        }
        else
        {
            return "There is an error";
        }

    }


    IEnumerator WaitToFinishDialog(Task task)
    {
        yield return new WaitForSeconds(3);
        task.Status = 1;
        PlayerManagement.player.CurrTask = task;
       
    }
}


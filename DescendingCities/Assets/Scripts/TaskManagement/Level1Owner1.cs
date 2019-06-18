using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1Owner1 : MonoBehaviour
{
    public Owner owner1;
    public Owner owner2;
    public Owner owner3;
    public Owner owner4;

    bool talkedWith2 = false;
    bool talkedWith3 = false;
    bool talkedWith4 = false;

    public NPCForInfo municipalityNPC;
    public NPCForInfo specialistNPC;

    public int step;

    public Text ToDoText;

    public string interactedName;

    void Start()
    {
        interactedName = "";
        step = 0;
    }

    void Update()
    {
        if(PlayerCollider.interactedPerson != null)
        {
            interactedName = PlayerCollider.interactedPerson.name;
          
        }
      
       if(owner1.Task.Status == 1)
        {
            if(step == 0)
            {
              
                ToDoText.text = "TO-DO:\nGo to municipality";
                municipalityNPC.sentence = "So you are looking for a way to finance foundation repairs.&We have something for you." +
                    "Starting a VvE is a great first step to share the burden between owners.&" +
                    "A Home Owner can privately get a loan of up to €65.000 through Energy Saving Loans&" +
                    "If you are part of a VvE, It is possible to get a loan of up to €2.500.000.";
                Debug.Log(interactedName);
                if(interactedName == municipalityNPC.gameObject.name)
                {
                    step = 1;
                }
                
            }
            }
            if(step == 1)
            {
                ToDoText.text = "TO-DO:\nReturn " + owner1.Name;
                municipalityNPC.sentence = "I hope this answers your question. Good luck!";
                owner1.Task.WaitingForSolution = "Thanks a lot. Starting a VVE is great idea!&I can be the first member!&Can you invite other owners as well?";
                if(interactedName == owner1.gameObject.name)
                {
                    PlayerManagement.currLevel.Vve.Add(owner1);
                    step = 2;
                    owner2.Task.TellPlayerHeIsBusy = "Thank you so much! Joining a VvE sounds great!";
                    owner3.Task.TellPlayerHeIsBusy = "Thank you so much! Joining a VvE sounds great!";
                    owner4.Task.TellPlayerHeIsBusy = "Thank you so much! Joining a VvE sounds great!";
            }
            }
            if(step == 2)
            {
                ToDoText.text = "TO-DO:\nInvite all owners to VvE";
                owner1.Task.WaitingForSolution = "I am looking forward for results";
                municipalityNPC.sentence = "Hi, What can I do for you?";

            if (interactedName == owner2.gameObject.name)
                {
                if (!PlayerManagement.currLevel.Vve.Contains(owner2)) {
                    PlayerManagement.currLevel.Vve.Add(owner2);

                }
                talkedWith2 = true;
                    owner2.Task.TellPlayerHeIsBusy = "You seem busy";
                }
                if (interactedName == owner3.gameObject.name)
                {
                if (!PlayerManagement.currLevel.Vve.Contains(owner3))
                {
                    PlayerManagement.currLevel.Vve.Add(owner3);

                }
                talkedWith3 = true;
                    owner3.Task.TellPlayerHeIsBusy = "You seem busy";
                }
                if (interactedName == owner4.gameObject.name)
                {
                if (!PlayerManagement.currLevel.Vve.Contains(owner4))
                {
                    PlayerManagement.currLevel.Vve.Add(owner4);

                }
                      talkedWith4 = true;
                      owner4.Task.TellPlayerHeIsBusy = "You seem busy";
                }
                if(talkedWith2 && talkedWith3 && talkedWith4)
                {
                    step = 3;
                }
            }
            if(step == 3)
            {
                ToDoText.text = "TO-DO:\nReturn " + owner1.Name;
                owner1.Task.status = 2;
                step = 4;
            }
        }
        
    }


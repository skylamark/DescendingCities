﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1Owner3 : MonoBehaviour
{
    public Owner owner1;
    public Owner owner2;
    public Owner owner3;
    public Owner owner4;

    bool talkedWith1 = false;
    bool talkedWith2 = false;
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
        if (PlayerCollider.interactedPerson != null)
        {
            interactedName = PlayerCollider.interactedPerson.name;

        }

        if (owner3.Task.Status == 1)
        {
            if (step == 0)
            {

                ToDoText.text = "TO-DO:\nGo to municipality";
                municipalityNPC.sentence = "If you want to make archive research,&I can give you building blueprints and remodeling permits.&But you may need a specialist to understand them.";

                Debug.Log(interactedName);
                if (interactedName == municipalityNPC.gameObject.name)
                {
                    step = 1;
                }

            }
            if (step == 1)
            {
                ToDoText.text = "TO-DO:\nGo to specialist";
                municipalityNPC.sentence = "Hi, what a beautiful day!";
                specialistNPC.sentence = "According to these documents you need bla bla bla";

                if (interactedName == specialistNPC.gameObject.name)
                {
                    step = 2;
                }
            }
            if (step == 2)
            {
                ToDoText.text = "TO-DO:\nReturn " + owner3.Name;
                specialistNPC.sentence = "Hi, what a beautiful day!";
                owner3.Task.WaitingForSolution = "So, we need bla bla. &Can you please inform other owner about this?";
                if (interactedName == owner3.gameObject.name)
                {
                    step = 3;
                    owner1.Task.TellPlayerHeIsBusy = "Thanks for info, I think we need a expert to solve this.";
                    owner2.Task.TellPlayerHeIsBusy = "Thanks for info, I think we need a expert to solve this.";
                    owner4.Task.TellPlayerHeIsBusy = "Thanks for info, I think we need a expert to solve this.";
                }
            }
            if (step == 3)
            {
                ToDoText.text = "TO-DO:\nInform all owners";
                owner3.Task.WaitingForSolution = "I am looking forward for results";

                if (interactedName == owner1.gameObject.name)
                {
                    talkedWith1 = true;
                    owner1.Task.TellPlayerHeIsBusy = "You seem busy";
                }
                if (interactedName == owner2.gameObject.name)
                {
                    talkedWith2 = true;
                    owner2.Task.TellPlayerHeIsBusy = "You seem busy";
                }
                if (interactedName == owner4.gameObject.name)
                {
                    talkedWith4 = true;
                    owner4.Task.TellPlayerHeIsBusy = "You seem busy";
                }
                if (talkedWith1 && talkedWith2 && talkedWith4)
                {
                    step = 4;
                }
            }
            if (step == 4)
            {
                ToDoText.text = "TO-DO:\nReturn " + owner3.Name;
                owner3.Task.status = 2;
            }
        }

    }
}

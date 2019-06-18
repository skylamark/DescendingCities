using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1Owner2 : MonoBehaviour
{
   
    public Owner owner2;
  
    
    public NPCForInfo concratorNPC;

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

        if (owner2.Task.Status == 1)
        {
            if (step == 0)
            {

                ToDoText.text = "TO-DO:\nGo to a contractor";
                concratorNPC.sentence = "So you have some problems with cracks on shared walls.&" +
                    "This is due to different types of foundation between the buildings, one sinks faster than the other.&" +
                    "Rebuild foundation to match the neighbours’ or separate buildings and rebuild walls. So they affect each other.";

                if (interactedName == concratorNPC.gameObject.name)
                {
                    step = 1;
                }

            }
            if (step == 1)
            {
                ToDoText.text = "TO-DO:\nReturn " + owner2.Name;
                concratorNPC.sentence = "Hi, what a beautiful day!";
                owner2.Task.status = 2;
            }
            
          

        }

    }
}

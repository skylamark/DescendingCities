using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Task: MonoBehaviour
{
    public int TaskID;
    [TextArea (5,10)]
    public string InitialConversation;
    [TextArea(5, 10)]
    public string TellPlayerHeIsBusy;
    [TextArea(5, 10)]
    public string CompletionMessage;
    [TextArea(5, 10)]
    public string WaitingForSolution;
    [TextArea(5, 10)]
    public string GenericResponse;
    public string location;
    public int status = 0;
    public string reward;
   


    public string Location { get => location; set => location = value; }
    public string Reward { get => reward; set => reward = value; }
    public int Status { get => status; set => status = value; }

 
}

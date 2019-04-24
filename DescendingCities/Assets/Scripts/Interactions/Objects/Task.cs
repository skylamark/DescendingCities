using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task: MonoBehaviour
{
    public string situation;
    public string location;
    public int status = 0;
    public string reward;
   

    public string Situation { get => situation; set => situation = value; }
    public string Location { get => location; set => location = value; }
    public string Reward { get => reward; set => reward = value; }
    public int Status { get => status; set => status = value; }
}

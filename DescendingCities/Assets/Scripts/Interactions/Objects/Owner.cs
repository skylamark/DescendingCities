using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Owner: MonoBehaviour 
{
    public string name;
    public Task task;

    public string Name { get => name; set => name = value; }
    public Task Task { get => task; set => task = value; }

   
}

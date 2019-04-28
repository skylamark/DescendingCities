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

    public int isInLevel(Level level)
    {
        //returns -1 if owner is not in level, 0 if it is in level and didn't give task, 1 if in level but already gave task

        if (level.Individuals.Contains(this))
        {
            return 0;
        }
        else if (level.Vve.Contains(this))
        {
            return 1;
        }
        else
        {
            return -1;
        }

    }
}

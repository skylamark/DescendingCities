using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable: MonoBehaviour
{
    private string effect;
    private string cause;
    private List<string> solutions;

    public string Effect { get => effect; set => effect = value; }
    public string Cause { get => cause; set => cause = value; }
    public List<string> Solution { get => solutions; set => solutions = value; }

    public List<string> isSolved(List<string> inventory)
    {
        List<string> possible = new List<string>();
        for(int i=0; i<solutions.Count; i++)
        {
            for(int j=0; j<inventory.Count; j++)
            {
                if (solutions[i].Equals(inventory[j]))
                    possible.Add(solutions[i]);
            }
        }
        return possible;
    }
}

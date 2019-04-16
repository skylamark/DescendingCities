using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    private List<Owner> individuals = new List<Owner>();
    private List<Owner> vve = new List<Owner>();

    public List<Owner> Individuals { get => individuals; set => individuals = value; }
    public List<Owner> Vve { get => vve; set => vve = value; }

    public bool isComplete()
    {
        if (Individuals.Count == 0)
            return true;
        else
            return false;
    }
}

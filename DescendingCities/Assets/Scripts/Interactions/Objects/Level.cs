﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level: MonoBehaviour
{
    public List<Owner> individuals = new List<Owner>();
    public List<Owner> vve = new List<Owner>();

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

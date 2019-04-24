﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player: MonoBehaviour
{
    private List<Collectable> problems;
    private List<string> inventory;
    private Task currTask = null;

    public List<Collectable> Problems { get => problems; set => problems = value; }
    public List<string> Inventory { get => inventory; set => inventory = value; }
    public Task CurrTask { get => currTask; set => currTask = value; }

    public bool isAvailable()
    {
        if (CurrTask == null)
            return true;
        else
            return false;
    }

   
    
}

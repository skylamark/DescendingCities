using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    private string situation;
    private string location;
    private bool activityComplete;
    private Reward rewardOfTask;
    private Owner owner;

    public string Situation { get => situation; set => situation = value; }
    public string Location { get => location; set => location = value; }
    public Reward RewardOfTask { get => rewardOfTask; set => rewardOfTask = value; }
    public bool ActivityComplete { get => activityComplete; set => activityComplete = value; }
    public Owner Owner { get => owner; set => owner = value; }

    public void carryOwner()
    {
        if (activityComplete)
        {
            PlayerManagement.player.CurrLevel.Individuals.Remove(owner);
            PlayerManagement.player.CurrLevel.Vve.Add(owner);
        }
    }
}

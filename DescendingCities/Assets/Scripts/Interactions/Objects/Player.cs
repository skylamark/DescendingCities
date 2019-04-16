using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player
{
    private Level currLevel;
    private string currProblem;
    private List<Reward> solutions = new List<Reward>();
    private Task currTask;

    public Level CurrLevel { get => currLevel; set => currLevel = value; }
    public List<Reward> Solutions { get => solutions; set => solutions = value; }
    public Task CurrTask { get => currTask; set => currTask = value; }
    public string CurrProblem { get => currProblem; set => currProblem = value; }

    public bool isBusy()
    {
        if (CurrTask == null)
            return false;
        else
            return true;
    }
    public string findSolution()
    {
        if (solutions != null) {
            for (int i = 0; i < solutions.Count; i++)
            {
                if (solutions.ElementAt(i).Problem.Equals(currProblem))
                    return solutions.ElementAt(i).RewardOfTask;
            }
            return "You have no solution for this problem. Hint: Go and help other owners.";
        }
        return "You have no solution for this problem. Hint: Go and help other owners.";
    }

    
}

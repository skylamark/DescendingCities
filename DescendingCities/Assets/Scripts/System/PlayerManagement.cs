using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public static Player player;
    public static Level level;
    public static Owner owner;
    public static Task task;
    public static Reward reward;
    public static string problem;
    

    void Start()
    {
       
            problem = "You have a crack on your wall";
            reward = new Reward();
            reward.Problem = problem;
            reward.RewardOfTask = "Call repairman that your neighbour suggested";
            owner = new Owner();
            owner.Name = "Epic";
            task = new Task();
            task.Situation = "Press the button";
            task.Location = "Game Scene;";
            task.RewardOfTask = reward;
            task.ActivityComplete = false;
            task.Owner = owner;
            level = new Level();
            level.Individuals = new List<Owner>() { owner };
            player = new Player();
            player.CurrLevel = level;
            player.CurrProblem = problem;
             

    }

    private void Update()
    {
        if (player.isBusy() && player.CurrTask.ActivityComplete)
        {
            player.CurrLevel.Individuals.Remove(owner);
            player.CurrLevel.Vve.Add(owner);
            Debug.Log(owner.Name + " joined vve");
            player.Solutions.Add(player.CurrTask.RewardOfTask);
            Debug.Log("you earned a reward");
        }
        if (player.CurrLevel.isComplete())
        {
            Debug.Log("Level is completed");
        }
    }

    public static void giveTask()
    {
        if (!player.isBusy())
        {
            player.CurrTask = task;
            
        }

        
    }
   
}

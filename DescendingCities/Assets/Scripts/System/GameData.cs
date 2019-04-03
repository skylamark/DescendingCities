using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]

public class GameData
{
  
    private int currentScene;
    private PlayerData player;

    public GameData(PlayerData player)
    {
        this.CurrentScene = SceneManager.GetActiveScene().buildIndex;
        this.Player = player;
    }

    public int CurrentScene { get => currentScene; set => currentScene = value; }
    public PlayerData Player { get => player; set => player = value; }
}

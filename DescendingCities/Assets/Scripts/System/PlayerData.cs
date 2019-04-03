using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    private float positionX;
    private float positionY;
    private float positionZ;
    private float speed;
    private bool died;

    public PlayerData(PlayerDirectional player)
    {
        this.PositionX = player.playerCharacterObject.transform.position.x;
        this.PositionY = player.playerCharacterObject.transform.position.y;
        this.PositionZ = player.playerCharacterObject.transform.position.z;
        this.Died = player.died;
        this.Speed = player.speed;
    }

    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public float PositionZ { get => positionZ; set => positionZ = value; }
    public bool Died { get => died; set => died = value; }
    public float Speed { get => speed; set => speed = value; }
}

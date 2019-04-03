using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoad
{
    public static void SaveGame(PlayerDirectional player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gameData.txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData playerData = new PlayerData(player);
        GameData gameData = new GameData(playerData);

        formatter.Serialize(stream, gameData);
        stream.Close();

    }

    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/gameData.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData gameData = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return gameData;

        }
        else
        {
            Debug.Log("Game initializing for first time");
            return null;
        }
    }
}

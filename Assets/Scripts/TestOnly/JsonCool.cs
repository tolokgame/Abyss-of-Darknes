/*using System;
using System.IO;
using UnityEngine;

public class JsonCool : MonoBehaviour
{
    public void Start()
    {
        SavePlayerData();
    }

    public void SavePlayerData()
    {
        PlayerData testPlayer = new PlayerData("Oleg", 67, "asdasdasdasdasdasdasd");

        string strData = JsonUtility.ToJson(testPlayer);
        File.WriteAllText(Application.persistentDataPath + "/save.json", strData);
    }

    public void LoadPlayerData()
    {
        string strData = File.ReadAllText(Application.persistentDataPath + "/save.json");
        PlayerData.loadedPlayer = JsonUtility.FromJson<PlayerData>(strData);
        Debug.Log("Name:" + loadedPlayer.Name);
        Debug.Log("Level" + loadedPlayer.Level);
        Debug.Log("Description: " + loadedPlayer.Description);
    }

    [Serializable]
    public class PlayerData
    {
        public string Name;
        public int Level;
        public string Description;

        public PlayerData(string name, int level, string description)
        {
            Name = name;
            Level = level;
            Description = description;
        }
    }
*/



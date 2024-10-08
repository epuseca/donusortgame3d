using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]

public class DataHandle : MonoBehaviour
{
    private static string pathResourcesFileConfigUI;

    public static List<Dictionary<string, object>> AchievementData = new List<Dictionary<string, object>>();
    private void Awake()
    {
        pathResourcesFileConfigUI = Application.persistentDataPath + "/FileDataConfig.json";
        
        Init();
        // AchievementData = CSVReader.Read(CSVName.ACHIEVEMENT_CONFIG);
        // levelDataConfig = CSVReader.Read(CSVName.LEVEL_CONFIG);
        // //
        // ragDollData.Add("RagDoll", CSVReader.Read(CSVName.RAGDOLL_CSV));
        // ragDollData.Add("Android", CSVReader.Read(CSVName.ANDROID_CSV));
        // ragDollData.Add("Immortal", CSVReader.Read(CSVName.IMMORTAL_CSV));
        // ragDollData.Add("Green", CSVReader.Read(CSVName.GREEN_CSV));
        // ragDollData.Add("Orange", CSVReader.Read(CSVName.ORANGE_CSV));
        // ragDollData.Add("RainbowFriend", CSVReader.Read(CSVName.RAINBOWFRIEND_CSV));\
    }
    int[,] rowss = { 
    
    };
    public static void InitFilePlayerData()
    {
        if (!File.Exists(pathResourcesFileConfigUI))
        {
            AllDataLevel playerData = new AllDataLevel();
            string data = JsonUtility.ToJson(playerData);
            File.Create(pathResourcesFileConfigUI).Dispose();
            File.WriteAllText(pathResourcesFileConfigUI, data);
            //
            
        }
    }

    private void Init()
    {
        InitFilePlayerData();
    }
    public static AllDataLevel ReadFile(string filePath)
    {
        if (File.Exists(filePath)) // Use the provided filePath
        {
            string dataString = File.ReadAllText(filePath);
            AllDataLevel dataItem = JsonUtility.FromJson<AllDataLevel>(dataString);
            return dataItem;
        }
        Debug.LogError("No data found at " + filePath);
        return null;
    }

    public static void WriteFilePlayerData(AllDataLevel playerData)
    {
        string data = "";
        data = JsonUtility.ToJson(playerData);
        if (!File.Exists(pathResourcesFileConfigUI))
        {
            File.Create(pathResourcesFileConfigUI).Dispose();
        }

        File.WriteAllText(pathResourcesFileConfigUI, data);
    }
}

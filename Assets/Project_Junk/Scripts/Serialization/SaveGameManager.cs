
using UnityEngine;
using System.IO;

public static class SaveGameManager 
{
    public static SaveData currentSaveData = new SaveData();

    public const string SaveDirectory = "/SaveData/";
    public const string FileName = "/SaveGame.sav";

    public static bool Save()
    {

        var dir = Application.persistentDataPath + SaveDirectory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(currentSaveData, true);        
        File.WriteAllText(dir + FileName, json);
        GUIUtility.systemCopyBuffer = dir;
        return true;
    }

    public static void Load()
    {
        string fullPath = Application.persistentDataPath + SaveDirectory + FileName;

        SaveData tempData = new SaveData();
        
        if(File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            tempData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.LogError("Save file does not exists!");
        }

        currentSaveData = tempData;
    }
}

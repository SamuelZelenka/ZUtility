using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static readonly string SESSION_SAVE_DIRECTORY = Application.dataPath + "/Saves/";
    public static readonly string ENCRYPTION_KEY = "ZOOMIES";
    
    public static void SaveData(string fileName, DataContainer data)
    {
        if (!Directory.Exists(SESSION_SAVE_DIRECTORY))
        {
            Directory.CreateDirectory(SESSION_SAVE_DIRECTORY);
        }
        string json = JsonUtility.ToJson(data);

        string encryptedData = StringEncryption.Encrypt(json, ENCRYPTION_KEY);

        File.WriteAllText(SESSION_SAVE_DIRECTORY + fileName + ".json", encryptedData);
        Debug.Log($"Saved {fileName}");
    }

    public static T LoadData<T>(string fileName) where T : DataContainer
    {
        string data = GetSessionDataString(fileName);
        Debug.Log($"Loaded {fileName}");
        return JsonUtility.FromJson<T>(data);
    }

    public static string GetSessionDataString(string saveName)
    {
        saveName = SESSION_SAVE_DIRECTORY + saveName + ".json";
        if (File.Exists(saveName))
        {
            string rawData = File.ReadAllText(saveName);
            string decryptedData = StringEncryption.Decrypt(rawData, ENCRYPTION_KEY);
            return decryptedData;
        }
        return null;
    }
    public static bool DeleteFile(string fileName)
    {
        string path = SaveSystem.SESSION_SAVE_DIRECTORY + fileName + ".json";
        if (File.Exists(path))
        {
            File.Delete(path);
            return true;
        }
        return false;
    }
}

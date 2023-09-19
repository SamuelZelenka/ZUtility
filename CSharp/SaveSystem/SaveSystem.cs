using System.Text.Json;

public static class SaveSystem
{    
    public static void SaveData(string encryptionKey, string directory, string fileName, object data)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        string json = JsonSerializer.Serialize(data);

        string encryptedData = StringEncryption.Encrypt(json, encryptionKey);

        File.WriteAllText(directory + fileName + ".json", encryptedData);
    }

    public static T? LoadData<T>(string encryptionKey, string directory, string fileName)
    {
        string data = GetSessionDataString(encryptionKey, directory, fileName);
        return JsonSerializer.Deserialize<T>(data);
	}

    public static string GetSessionDataString(string encryptionKey, string directory, string saveName)
    {
        saveName = directory + saveName + ".json";
        if (File.Exists(saveName))
        {
            string rawData = File.ReadAllText(saveName);
            string decryptedData = StringEncryption.Decrypt(rawData, encryptionKey);
            return decryptedData;
        }
        return null;
    }
    public static bool DeleteFile(string directory, string fileName)
    {
        string path = directory + fileName + ".json";
        if (File.Exists(path))
        {
            File.Delete(path);
            return true;
        }
        return false;
    }
}

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadSystem
{
    public static bool Save(SaveData data)
    {
        string path = Application.persistentDataPath + "/SaveData.pb";

        FileStream stream = new FileStream(path, FileMode.Create);

        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(stream, data);

        stream.Close();

        return true;
    }

    public static SaveData Load()
    {
        string path = Application.persistentDataPath + "/SaveData.pb";

        if (File.Exists(path) == false)
        {
            Debug.LogWarning("File doesn't exist! " + path);
            return null;
        }

        FileStream stream = new FileStream(path, FileMode.Open);

        BinaryFormatter formatter = new BinaryFormatter();

        SaveData data;
        data = (SaveData)formatter.Deserialize(stream);

        stream.Close();

        return data;
    }
}

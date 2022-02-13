using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class FileDataLoader : MonoBehaviour
{
    protected string savePath;
    protected string saveName;

    bool IsFilePathThere()
    {
        return Directory.Exists(savePath);
    }

    protected void Save<T>(T data)
    {
        if (!IsFilePathThere())
            Directory.CreateDirectory(savePath);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveName);

        var json = JsonUtility.ToJson(data);
        bf.Serialize(file, json);
        file.Close();
    }

    protected void Load<T>(T data)
    {
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(saveName))
        {
            FileStream file = File.Open(saveName, FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), data);
            file.Close();
        }
    }
}

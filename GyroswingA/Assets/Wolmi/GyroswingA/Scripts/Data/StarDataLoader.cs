using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class StarDataLoader : MonoBehaviour
{
    [HideInInspector] public StarDataPerLevel data;

    string savePath;
    string saveName;


    void Awake()
    {
        savePath = Application.persistentDataPath + "/star_data";
        saveName = Application.persistentDataPath + "/star_data/star_data_save.txt";
    }

    void Start()
    {
        data = new StarDataPerLevel();
        data.Clear();
        LoadStarDataFile();
    }

    bool IsFilePathThere()
    {
        return Directory.Exists(savePath);
    }

    public void SaveStarDataFile()
    {
        if (!IsFilePathThere())
            Directory.CreateDirectory(savePath);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveName);

        var json = JsonUtility.ToJson(data);
        bf.Serialize(file, json);
        file.Close();
    }

    public void LoadStarDataFile()
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

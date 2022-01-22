using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class GameDataLoader
{
    static string path = Application.persistentDataPath + "/star_data";
    static string name = Application.persistentDataPath + "/star_data/star_data_save.txt";

    static bool IsFilePathThere()
    {
        return Directory.Exists(path);
    }

    public static void SaveStarDataFile(StarDataPerLevel starData)
    {
        if (!IsFilePathThere())
        {
            Directory.CreateDirectory(path);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(name);

        var json = JsonUtility.ToJson(starData);
        bf.Serialize(file, json);
        file.Close();
    }

    public static void LoadStarDataFile(StarDataPerLevel starData)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(name))
        {
            FileStream file = File.Open(name, FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), starData);
            file.Close();
        }
    }


}

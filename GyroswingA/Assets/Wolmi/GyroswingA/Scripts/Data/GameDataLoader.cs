using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameDataLoader : MonoBehaviour
{
    [SerializeField] StarDataPerLevel starData;

    static GameDataLoader instance;
    public static GameDataLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameDataLoader>();
            }
            return instance;
        }
    }

    void Awake()
    {
        var objs = FindObjectsOfType<GameDataLoader>();

        if (objs.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject); // destroy new obj
            return;
        }

        if (instance == null) instance = this;
        
    }

    bool IsFilePathThere()
    {
        return Directory.Exists(Application.persistentDataPath + "/star_data");
    }

    public void SaveFile()
    {
        if (!IsFilePathThere())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/star_data");
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/star_data/star_data_save.txt");

        var json = JsonUtility.ToJson(starData);
        bf.Serialize(file, json);
        file.Close();
    }

    public void LoadFile()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/star_data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/star_data");
        }
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + "/star_data/star_data_save.txt"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/star_data/star_data_save.txt",
                FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), starData);
            file.Close();
        }
    }


}

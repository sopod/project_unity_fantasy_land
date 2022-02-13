using UnityEngine;


public class StarDataLoader : FileDataLoader
{
    [HideInInspector] public StarDataPerLevel data;
    
    void Awake()
    {
        savePath = Application.persistentDataPath + "/star_data";
        saveName = Application.persistentDataPath + "/star_data/star_data_save.txt";
    }

    void Start()
    {
        data = new StarDataPerLevel();
        data.Clear();
        LoadStarData();
    }

    public void LoadStarData()
    {
        Load(data);
    }

    public void SaveStarData()
    {
        Save(data);
    }
    
}

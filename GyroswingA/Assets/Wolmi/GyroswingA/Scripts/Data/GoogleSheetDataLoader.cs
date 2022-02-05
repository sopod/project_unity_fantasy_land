using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;



public class GoogleSheetDataLoader : MonoBehaviour
{
    LevelValues[] levelDatas;

    bool levelValuesGetDone = false;
    public bool IsDataSet { get { return levelValuesGetDone; } }

    [SerializeField] string LevelValuesURL = "https://script.google.com/macros/s/AKfycbx0BbUOI8G5JZFt3ZihJf_HF1018MlMZqWgGJj0kbnoJ8Ml1joeqsUlJjq2znLtANkUHQ/exec";

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        StartCoroutine(GetGoogleSheetLevelValueData());
    }

    IEnumerator GetGoogleSheetLevelValueData()
    {
        UnityWebRequest www = UnityWebRequest.Get(LevelValuesURL);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("ERROR: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            json = AddWrapperJson(json);

            RawLevelValues[] output = FromJson<RawLevelValues>(json);
            SaveLevelData(output);

            levelValuesGetDone = true;
        }
    }
    
    IEnumerator PostGoogleSheetStarData(string url, string result)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(result);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("ERROR: " + request.error);
        }
    }
    
    string AddWrapperJson(string value)
    {
        string result = "{\"Values\":" + value + "}";
        return result;
    }
    
    string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Values = array;
        return JsonUtility.ToJson(wrapper, false);
    }

    T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Values;
    }

    T[] GetTypes<T>(string input)
    {
        if (input == "") return new T[] { };

        string[] result = input.Split(',');

        T[] es = new T[result.Length];

        for (int i = 0; i < result.Length; i++)
        {
            es[i] = (T)Enum.Parse(typeof(T), result[i]);
        }

        return es;
    }

    void SaveLevelData(RawLevelValues[] output)
    {
        levelDatas = new LevelValues[output.Length];

        for (int i = 0; i < output.Length; i++)
        {
            levelDatas[i] = new LevelValues();

            levelDatas[i].Level = output[i].SwingSpeed;
            levelDatas[i].SwingSpeed = output[i].SwingSpeed;
            levelDatas[i].SwingAngleMax = output[i].SwingAngleMax;
            levelDatas[i].SpinSpeed = output[i].SpinSpeed;
            levelDatas[i].EnemyTypes = GetTypes<EnemyType>(output[i].EnemyTypes);
            levelDatas[i].ItemTypes = GetTypes<ItemType>(output[i].ItemTypes);
        }
    }

    public LevelValues GetLevelValueCur(GameMode mode, int level)
    {
        int lev = (level - 1) + (int)mode * 10;

        return levelDatas[lev];
    }
}

[System.Serializable]
public class Wrapper<T>
{
    public T[] Values;
}

[System.Serializable]
public class RawLevelValues
{
    public int Level;
    public int SwingSpeed;
    public int SwingAngleMax;
    public int SpinSpeed;
    public string EnemyTypes;
    public string ItemTypes;
}

[System.Serializable]
public class LevelValues
{
    public int Level;
    public int SwingSpeed;
    public int SwingAngleMax;
    public int SpinSpeed;
    public EnemyType[] EnemyTypes;
    public ItemType[] ItemTypes;
}

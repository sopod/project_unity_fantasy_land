using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;



public class GoogleSheetDataLoader : MonoBehaviour
{
    LevelValues[] levelDatas;
    [SerializeField] ObjectValues objectDatas;
    public ObjectValues ObjectDatas { get => objectDatas; }

    bool hasGotLevelValues = false;
    bool hasGotObjectValues = false;
    public bool hasGotDatas { get { return hasGotLevelValues && hasGotObjectValues; } }

    [SerializeField] string levelValuesURL = "https://script.google.com/macros/s/AKfycbx0BbUOI8G5JZFt3ZihJf_HF1018MlMZqWgGJj0kbnoJ8Ml1joeqsUlJjq2znLtANkUHQ/exec";
    [SerializeField] string objectValuesURL = "https://script.google.com/macros/s/AKfycby4TJT8GxeJufPAsCyr2u9s73GiBAsJhA9Oei8sYfqBllctTL4gme1uWYNCm-w23YuB_A/exec";

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        StartCoroutine(GetLevelValuesData());
        StartCoroutine(GetObjectValuesData());
    }

    IEnumerator GetLevelValuesData()
    {
        UnityWebRequest www = UnityWebRequest.Get(levelValuesURL);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogWarning("ERROR: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            json = AddWrapperJson(json);

            RawLevelValues[] output = FromJsonAsList<RawLevelValues>(json);
            SaveLevelData(output);

            hasGotObjectValues = true;
        }
    }

    IEnumerator GetObjectValuesData()
    {
        UnityWebRequest www = UnityWebRequest.Get(objectValuesURL);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogWarning("ERROR: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            json = AddWrapperJson(json);

            objectDatas = new ObjectValues();
            objectDatas = FromJson<ObjectValues>(json);

            hasGotLevelValues = true;
        }
    }

    IEnumerator PostData(string url, string result)
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
            Debug.LogWarning("ERROR: " + request.error);
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

    T FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Values[0];
    }

    T[] FromJsonAsList<T>(string json)
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


[System.Serializable]
public class ObjectValues
{
    public float MoveSpeed;
    public float RotateSpeed;
    public float JumpPower;

    public float SkillCoolTime;
    public float DashPowerToDamaged;
    public float DashPowerToHit;
    public float FireBallPowerToDamaged;
    public float KnockDownTime;

    public float EnemyWaitTime; 
    public float EnemyTurnTime; 
    public float EnemyMoveTime; 
    public float EnemyLongTurnTime; 
}



//public float Gravity = 9.8f;

//public float MoveSpeed = 2.0f;
//public float RotateSpeed = 150.0f;
//public float JumpPower = 5.0f;

//public float SkillCoolTime = 0.5f;
//public float DashPowerToDamaged = 10.0f;
//public float DashPowerToHit = 20.0f;
//public float FireBallPowerToDamaged = 20.0f;
//public float KnockDownTime = 2.0f;

//public float EnemyWaitTime = 1.0f;
//public float EnemyTurnTime = 0.3f;
//public float EnemyMoveTime = 0.5f;
//public float EnemyLongTurnTime = 0.5f;

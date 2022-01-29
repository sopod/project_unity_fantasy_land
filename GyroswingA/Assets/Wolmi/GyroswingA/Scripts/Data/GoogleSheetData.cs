using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetData : MonoBehaviour
{
    LevelValues[] leveldatas;

    bool doneFlag = false;
    public bool IsDataSet { get { return doneFlag; } }

    private string url =
        "https://script.googleusercontent.com/macros/echo?user_content_key=LfDt42V0EjwepjdNSpmRQzX_BEqhuTG0PWfeJah-tfSrmfzUwu_opzcSiVYwg5Jmx8SMbmNnApoZ3FSgBgCeuRl3yr-9H33Dm5_BxDlH2jW0nuo2oDemN9CCS2h10ox_1xSncGQajx_ryfhECjZEnAZ8G9mW9IeVeFxWTjChNjWB_1KBvfuR9ZJxiVGCI58nUjUHOAElkCsBg1Np9ZdxinaO3Sq9cf8S5A-BL52_kcR4zyVmjAjQXNz9Jw9Md8uu&lib=Mopfy0-hgcm3Ojs5VrYx8X3qTP9lzMDak";

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        StartCoroutine(GetGoogleSheetData());
    }

    IEnumerator GetGoogleSheetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("ERROR: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            json = FixJson(json);

            RawLevelValues[] output = FromJson<RawLevelValues>(json);
            SaveLevelData(output);

            doneFlag = true;
        }
    }

    string FixJson(string value)
    {
        value = "{\"Values\":" + value + "}";
        return value;
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
        leveldatas = new LevelValues[output.Length];

        for (int i = 0; i < output.Length; i++)
        {
            leveldatas[i] = new LevelValues();

            leveldatas[i].Level = output[i].SwingSpeed;
            leveldatas[i].SwingSpeed = output[i].SwingSpeed;
            leveldatas[i].SwingAngleMax = output[i].SwingAngleMax;
            leveldatas[i].SpinSpeed = output[i].SpinSpeed;
            leveldatas[i].EnemyTypes = GetTypes<EnemyType>(output[i].EnemyTypes);
            leveldatas[i].ItemTypes = GetTypes<ItemType>(output[i].ItemTypes);
        }
    }

    public LevelValues GetLevelValueCur(GameMode mode, int level)
    {
        int lev = level + (int)mode * 10;

        return leveldatas[lev];
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

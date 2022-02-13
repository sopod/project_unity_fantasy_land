using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


// 손쉬운 레벨 디자인을 위해 Google Spread Sheet를 사용하여 데이터를 로드하였습니다. 


public class GoogleSheetDataLoader : MonoBehaviour
{
    [SerializeField] LevelValues[] levelValues;
    [SerializeField] ObjectValues objectDatas;
    public ObjectValues ObjectDatas { get => objectDatas; }

    bool hasLevelValuesLoaded = false;
    bool hasObjectValuesLoaded = false;
    public bool HasDataLoaded { get { return hasLevelValuesLoaded && hasObjectValuesLoaded; } }

    [SerializeField] string levelValuesURL;
    [SerializeField] string objectValuesURL;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        StartCoroutine(GetData(true));
        StartCoroutine(GetData(false));
    }

    IEnumerator GetData(bool isLevelData)
    {
        UnityWebRequest www = (isLevelData) ? UnityWebRequest.Get(levelValuesURL) : UnityWebRequest.Get(objectValuesURL);

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

            if (isLevelData)
            {
                RawLevelValues[] output = FromJsonAsList<RawLevelValues>(json);
                SaveLevelData(output);

                hasObjectValuesLoaded = true;
            }
            else
            {
                objectDatas = new ObjectValues();
                objectDatas = FromJson<ObjectValues>(json);

                hasLevelValuesLoaded = true;
            }
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
    
    // 얻어온 string값을 json으로 파싱하기 전에, 형식을 맞춰주기 위해 문자를 덧붙입니다. 
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

    // string 데이터를 아이템이나 적 타입의 enum으로 파싱합니다.  
    T[] GetTypes<T>(string input)
    {
        if (input == "") return new T[] { };

        string[] result = input.Split(',');

        T[] types = new T[result.Length];
        for (int i = 0; i < result.Length; i++)
        {
            types[i] = (T)Enum.Parse(typeof(T), result[i]);
        }

        return types;
    }

    // 파싱한 타입으로 데이터에 저장합니다. 
    void SaveLevelData(RawLevelValues[] output)
    {
        levelValues = new LevelValues[output.Length];

        for (int i = 0; i < output.Length; i++)
        {
            levelValues[i] = new LevelValues();

            levelValues[i].Level = output[i].SwingSpeed;
            levelValues[i].SwingSpeed = output[i].SwingSpeed;
            levelValues[i].SwingAngleMax = output[i].SwingAngleMax;
            levelValues[i].SpinSpeed = output[i].SpinSpeed;
            levelValues[i].EnemyTypes = GetTypes<EnemyType>(output[i].EnemyTypes);
            levelValues[i].ItemTypes = GetTypes<ItemType>(output[i].ItemTypes);
        }
    }

    public LevelValues GetCurrentStageValue(GameMode mode, int level)
    {
        int lev = (level - 1) + (int)mode * 10;
        return levelValues[lev];
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

    public int EasyModeMonsterMax;
    public int HardModeMonsterMax;
    public float SwookSpeedUp;
    public float GumSpeedUp;
    public float SwookDashUp;
    public float HarippoBlueTime;
    public float HarippoGreenTime;
    public float HarippoYellowTime;
    public float HarippoRedTime;
    public float CokeSpeed;
    public float ChocoTarteSpeed;


    public float GetEnemyMoveSpeed(EnemyType type, float playerMoveSpeed)
    {
        switch (type)
        {
            case EnemyType.Juck:
            case EnemyType.Swook: return playerMoveSpeed * SwookSpeedUp;
            case EnemyType.Gum: return playerMoveSpeed * GumSpeedUp;
        }
        return playerMoveSpeed;
    }

    public float GetDashPowerToDamagedByEnemy(EnemyType type, float dashPowerToDamaged)
    {
        switch (type)
        {
            case EnemyType.Juck:
            case EnemyType.Swook: return dashPowerToDamaged * SwookDashUp;
        }
        return dashPowerToDamaged;
    }

    public float GetTimeToUpgradeByItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.HarippoBlue: return HarippoBlueTime;
            case ItemType.HarippoGreen: return HarippoGreenTime;
            case ItemType.HarippoYellow: return HarippoYellowTime;
            case ItemType.HarippoRed: return HarippoRedTime;
        }
        return 0.0f;
    }

    public float GetSpeedToUpgradeByItem(ItemType type, float playerMoveSpeed)
    {
        switch (type)
        {
            case ItemType.Coke: return playerMoveSpeed * CokeSpeed;
            case ItemType.ChocoTarte: return playerMoveSpeed * ChocoTarteSpeed;
        }

        return playerMoveSpeed;
    }
}
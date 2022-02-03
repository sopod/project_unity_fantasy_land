using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SceneState
{
    Loading,
    Lobby,
    StageSelection,
    InGame,
    Max
}

public class SceneController : MonoBehaviour
{
    string[] sceneName = {"Loader", "Lobby", "StageSelection", "InGame"};
    SceneState curSceneState = SceneState.Loading;
    public SceneState CurScene { get { return curSceneState; } }

    bool playLobbySceneMusic = true;
    public bool PlayLobbySceneMusic { get { return playLobbySceneMusic; } }
    
    bool isDataLoaded = false;

    public GoogleSheetDataLoader loaderLevelValues;
    public StarDataLoader loaderStarData;

    static SceneController instance;
    public static SceneController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<SceneController>();
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance == null)
            instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        UISoundPlayer.Instance.PlayBGM(SceneState.Lobby);

        ChangeScene(SceneState.Lobby);
    }

    public void ChangeSceneToMainGame()
    {
        if (!isDataLoaded)
        {
            StartCoroutine(WaitUntilDataSet());
        }
        else
        {
            ChangeScene(SceneState.InGame);
        }
    }


    IEnumerator WaitUntilDataSet()
    {
        while (true)
        {
            if (loaderLevelValues.IsDataSet) break;

            yield return null;
        }

        isDataLoaded = true;
        ChangeScene(SceneState.InGame);
    }


    public void ChangeScene(SceneState toChange)
    {
        playLobbySceneMusic = (CurScene == SceneState.InGame);

        curSceneState = toChange;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName[(int)toChange]);
    }

    public void SaveFileAndQuitGame()
    {
        loaderStarData.SaveStarDataFile();
        Application.Quit();
    }
}

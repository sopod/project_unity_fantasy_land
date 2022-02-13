using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneState
{
    Loader,
    Lobby,
    StageSelection,
    InGame,
    Loading,
    Max
}

public class SceneController : MonoBehaviour
{
    string[] sceneName = {"Loader", "Lobby", "StageSelection", "InGame", "Loading"};
    SceneState curSceneState = SceneState.Loader;
    public SceneState CurScene { get { return curSceneState; } }
    SceneState nextSceneState;
    Slider loadingSceneSilder;

    bool playLobbySceneMusic = true;
    public bool PlayLobbySceneMusic { get { return playLobbySceneMusic; } }
    
    public GoogleSheetDataLoader loaderGoogleSheet;
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
        ChangeScene(SceneState.Loading);

        nextSceneState = SceneState.InGame;
    }

    public void DoLoadingSceneProcess(Slider slider)
    {
        loadingSceneSilder = slider;
        StartCoroutine(ToInGameProcess());
    }

    IEnumerator ToInGameProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName[(int)nextSceneState]);
        op.allowSceneActivation = false;
        float timer = 0.0f;

        while(!op.isDone)
        {
            if (loadingSceneSilder.value < 0.9f)
            {
                timer += (Time.unscaledDeltaTime * 0.1f);
                loadingSceneSilder.value = Mathf.Lerp(0f, 1f, timer);
            }

            if (loadingSceneSilder.value > 0.9f && loaderGoogleSheet.hasGotDatas)
            {
                playLobbySceneMusic = (CurScene == SceneState.InGame);
                curSceneState = nextSceneState;
                loadingSceneSilder.value = 1f;
                op.allowSceneActivation = true;
                yield break;
            }

            yield return null;
        };
    }


    public void ChangeScene(SceneState toChange)
    {
        playLobbySceneMusic = (CurScene == SceneState.InGame);

        curSceneState = toChange;
        SceneManager.LoadScene(sceneName[(int)toChange]);
    }

    public void SaveFileAndQuitGame()
    {
        loaderStarData.SaveStarData();
        Application.Quit();
    }
}

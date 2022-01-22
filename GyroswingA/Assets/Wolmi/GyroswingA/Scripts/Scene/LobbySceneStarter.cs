using UnityEngine;

public class LobbySceneStarter : MonoBehaviour
{
    [SerializeField] UISoundPlayer uiSoundPlayer;
    [SerializeField] VolumeSlider BgmSlider;
    [SerializeField] VolumeSlider effectSoundSlider;
    [SerializeField] StarDataPerLevel data;


    void Start()
    {
        GameDataLoader.LoadStarDataFile(data);

        uiSoundPlayer.PlayBGM(GameState.Lobby);

        BgmSlider.InitSlider();
        effectSoundSlider.InitSlider();
    }


    // --------------------- reset data button for test
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            data.Clear();

        if (Input.GetKeyDown(KeyCode.M))
            data.UnlockAll();
    }
}

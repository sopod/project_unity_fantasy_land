using UnityEngine;

public class LobbySceneStarter : MonoBehaviour
{
    [SerializeField] UISoundPlayer uiSoundPlayer;
    [SerializeField] VolumeSlider BgmSlider;
    [SerializeField] VolumeSlider effectSoundSlider;

    void Start()
    {
        GameDataLoader.Instance.LoadFile();

        uiSoundPlayer.PlayBGM(GameState.Lobby);

        BgmSlider.InitSlider();
        effectSoundSlider.InitSlider();
    }


    // --------------------- reset data button for test
    [SerializeField] StarDataPerLevel data;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            data.Clear();
    }
}

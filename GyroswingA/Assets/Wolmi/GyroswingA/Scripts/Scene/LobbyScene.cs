using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] VolumeSlider BgmSlider;
    [SerializeField] VolumeSlider effectSoundSlider;
    [SerializeField] StarDataPerLevel starData;


    void Start()
    {
        BgmSlider.InitSlider();
        effectSoundSlider.InitSlider();

        if (SceneController.Instance.PlayLobbySceneMusic)
            UISoundPlayer.Instance.PlayBGM(SceneState.Lobby);
    }


    // --------------------- reset data button for test
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            starData.Clear();

        if (Input.GetKeyDown(KeyCode.M))
            starData.UnlockAll();
    }
}

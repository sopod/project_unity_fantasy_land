using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] VolumeSlider BgmSlider;
    [SerializeField] VolumeSlider effectSoundSlider;


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
            SceneController.Instance.loaderStarData.data.Clear();

        if (Input.GetKeyDown(KeyCode.M))
            SceneController.Instance.loaderStarData.data.UnlockAll();
    }
}

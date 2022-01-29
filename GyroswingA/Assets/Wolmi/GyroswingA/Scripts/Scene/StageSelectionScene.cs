using UnityEngine;

public class StageSelectionScene : MonoBehaviour
{
    void Start()
    {
        if (SceneController.Instance.PlayLobbySceneMusic)
            UISoundPlayer.Instance.PlayBGM(SceneState.Lobby);
    }
}

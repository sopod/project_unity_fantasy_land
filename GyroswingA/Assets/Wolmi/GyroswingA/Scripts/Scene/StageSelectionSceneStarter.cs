using UnityEngine;

public class StageSelectionSceneStarter : MonoBehaviour
{
    [SerializeField] UISoundPlayer uiSoundPlayer;
    void Start()
    {
        uiSoundPlayer.PlayBGM(GameState.Lobby);
    }
}

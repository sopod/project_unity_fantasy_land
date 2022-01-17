using UnityEngine;

public class LobbySceneStarter : MonoBehaviour
{
    [SerializeField] UISoundPlayer uiSoundPlayer;

    void Start()
    {
        uiSoundPlayer.PlayBGM(GameState.Lobby);
    }


    // --------------------- reset data button for test
    [SerializeField] StarDataPerLevel data;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            data.Clear();
    }
}

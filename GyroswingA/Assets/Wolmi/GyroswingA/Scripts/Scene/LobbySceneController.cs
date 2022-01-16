using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneController : MonoBehaviour
{
    [SerializeField] UISoundPlayer uiSoundPlayer;

    void Start()
    {
        uiSoundPlayer.PlayBGM(GameState.Lobby);
    }
}

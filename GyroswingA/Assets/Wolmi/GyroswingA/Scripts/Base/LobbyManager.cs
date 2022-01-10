using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    GameState gameState;
    [SerializeField] UISoundPlayer uiSoundPlayer;


    static LobbyManager instance;
    public static LobbyManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<LobbyManager>();
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;

    }

    void Start()
    {
        uiSoundPlayer.PlayBGM(gameState);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIDisplay : MonoBehaviour
{
    [SerializeField] UIButton playButton;
    [SerializeField] UIButton optionsButton;
    [SerializeField] UIButton closeOptionButton;
    [SerializeField] UIButton exitGameButton;
    [SerializeField] GameObject optionScreen;

    public void Start()
    {
         UIEventMaker.MakeButtonEvent(playButton);
         UIEventMaker.MakeButtonEvent(optionsButton);
         UIEventMaker.MakeButtonEvent(closeOptionButton);
         UIEventMaker.MakeButtonEvent(exitGameButton);
        
         optionScreen.SetActive(false);
    }
}

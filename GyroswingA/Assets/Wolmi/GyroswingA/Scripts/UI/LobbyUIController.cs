using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIController : MonoBehaviour
{
    [SerializeField] UIButton playButton;
    [SerializeField] UIButton optionsButton;

    public void Start()
    {
         UIEventMaker.MakeUIObjectWork(playButton);
         UIEventMaker.MakeUIObjectWork(optionsButton);
    }
}

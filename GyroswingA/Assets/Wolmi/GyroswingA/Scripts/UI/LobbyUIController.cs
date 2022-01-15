using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIController : MonoBehaviour
{
    [SerializeField] UIButton playButton;
    [SerializeField] UIButton pauseButton;

    public void Start()
    {
        UIEventMaker maker = new UIEventMaker();
        maker.MakeUIObjectWork(playButton);
        maker.MakeUIObjectWork(pauseButton);
    }
}

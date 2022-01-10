using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIController : MonoBehaviour
{
    [SerializeField] UIButton playButton;

    public void Start()
    {
        UIEventMaker maker = new UIEventMaker();
        maker.MakeUIObjectWork(playButton);
    }
}

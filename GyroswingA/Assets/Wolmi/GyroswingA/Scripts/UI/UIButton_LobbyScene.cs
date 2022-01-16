using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton_LobbyScene : UIButton
{
    public override void OnClicked()
    {
        SceneLoader.LoadScene("Lobby");
    }
}

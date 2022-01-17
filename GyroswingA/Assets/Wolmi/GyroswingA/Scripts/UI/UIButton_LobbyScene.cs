using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton_LobbyScene : UIButton
{
    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);

        if (SceneManager.GetActiveScene().name == "InGame")
            Invoke("BackToLobby" , 1.0f);
        else
            BackToLobby();
    }

    void BackToLobby()
    {
        SceneLoader.LoadScene("Lobby");
    }
}

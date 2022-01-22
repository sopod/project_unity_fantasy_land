using UnityEngine.SceneManagement;

public class UIButton_LobbyScene : UIButton
{
    void Start()
    {
        UIEventMaker.MakeButtonEvent(this);
    }
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

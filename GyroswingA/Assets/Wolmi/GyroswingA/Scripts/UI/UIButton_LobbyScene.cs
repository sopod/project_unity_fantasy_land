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

        if (SceneController.Instance.CurScene == SceneState.InGame)
            Invoke("BackToLobby" , 1.0f);
        else
            BackToLobby();
    }

    void BackToLobby()
    {
        SceneController.Instance.ChangeScene(SceneState.Lobby);
    }
}

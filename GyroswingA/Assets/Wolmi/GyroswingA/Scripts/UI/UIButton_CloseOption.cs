using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton_CloseOption : UIButton
{
    [SerializeField] GameCenter gameCenter;
    [SerializeField] private InGameUIDisplay display;
    [SerializeField] GameObject optionScreen;

    void Start()
    {
        UIEventMaker.MakeButtonEvent(this);
    }

    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnBack);

        if (SceneController.Instance.CurScene == SceneState.InGame)
        {
            display.SetGameUI();
            gameCenter.MakeObjectsStartMoving();
        }
        else
        {
            optionScreen.SetActive(false);
        }
    }
}

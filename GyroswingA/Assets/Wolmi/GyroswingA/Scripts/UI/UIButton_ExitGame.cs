using UnityEngine;

public class UIButton_ExitGame : UIButton
{

    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);

        Invoke("QuitGame", 1.0f);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}

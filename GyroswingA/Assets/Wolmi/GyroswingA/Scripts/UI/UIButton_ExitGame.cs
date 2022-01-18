using UnityEngine;

public class UIButton_ExitGame : UIButton
{

    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);

        GameDataLoader.Instance.SaveFile();

        Invoke("QuitGame", 1.0f);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}

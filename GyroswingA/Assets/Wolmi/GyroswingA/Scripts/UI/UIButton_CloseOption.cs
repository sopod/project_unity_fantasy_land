using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton_CloseOption : UIButton
{
    [SerializeField] GameManager manager;
    [SerializeField] private InGameUIDisplay display;
    [SerializeField] GameObject optionScreen;

    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnBack);

        if (SceneManager.GetActiveScene().name == "InGame")
        {
            display.SetGameUI();
            manager.SetStartMoving();
        }
        else
        {
            optionScreen.SetActive(false);
        }
    }
}

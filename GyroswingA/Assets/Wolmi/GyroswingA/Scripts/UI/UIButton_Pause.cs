using UnityEngine;

public class UIButton_Pause : UIButton
{
    [SerializeField] GameManager manager;
    [SerializeField] InGameUIDisplay display;

    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);

        manager.SetPauseMoving();
        display.SetOptionUI();
    }
}

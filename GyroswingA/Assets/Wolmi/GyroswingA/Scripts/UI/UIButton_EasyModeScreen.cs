using UnityEngine;

public class UIButton_EasyModeScreen : UIButton
{
    [SerializeField] StageSelectionUIDisplay ui;

    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);

        ui.SetEasyModeUI();
    }
}

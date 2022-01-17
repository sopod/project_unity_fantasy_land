using UnityEngine;

public class UIButton_HardModeScreen : UIButton
{
    [SerializeField] StageSelectionUIDisplay ui;
    
    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);
        
        ui.SetHardModeUI();
    }
}

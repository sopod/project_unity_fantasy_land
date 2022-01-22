using UnityEngine;

public class UIButton_HardModeScreen : UIButton
{
    [SerializeField] StageSelectionUIDisplay ui;
    void Start()
    {
        UIEventMaker.MakeButtonEvent(this);
    }
    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);
        
        ui.SetHardModeUI();
    }
}

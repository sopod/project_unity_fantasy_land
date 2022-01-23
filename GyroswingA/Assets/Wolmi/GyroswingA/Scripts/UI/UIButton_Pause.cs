using UnityEngine;

public class UIButton_Pause : UIButton
{
    [SerializeField] GameCenter _center;
    [SerializeField] InGameUIDisplay display;
    void Start()
    {
        UIEventMaker.MakeButtonEvent(this);
    }

    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);

        _center.SetPauseMoving();
        display.SetOptionUI();
    }
}

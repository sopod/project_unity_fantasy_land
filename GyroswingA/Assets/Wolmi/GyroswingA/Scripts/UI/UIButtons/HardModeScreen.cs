using UnityEngine;

namespace UIButtons
{
    public class HardModeScreen : UIButton
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
}
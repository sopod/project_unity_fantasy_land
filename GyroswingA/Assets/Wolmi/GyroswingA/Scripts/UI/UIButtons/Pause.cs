using UnityEngine;

namespace UIButtons
{
    public class Pause : UIButton
    {
        [SerializeField] GameCenter gameCenter;
        [SerializeField] InGameUIDisplay display;

        void Start()
        {
            UIEventMaker.MakeButtonEvent(this);
        }

        public override void OnClicked()
        {
            UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);

            gameCenter.MakeObjectPaused();
            display.SetOptionUI();
        }
    }
}
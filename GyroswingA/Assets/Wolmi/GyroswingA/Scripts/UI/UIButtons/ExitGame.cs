using UnityEngine;

namespace UIButtons
{
    public class ExitGame : UIButton
    {
        void Start()
        {
            UIEventMaker.MakeButtonEvent(this);
        }

        public override void OnClicked()
        {
            UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);
            SceneController.Instance.SaveFileAndQuitGame();
        }
    }
}


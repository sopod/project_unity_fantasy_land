using UnityEngine;

namespace UIButtons
{
    public class Option : UIButton
    {
        [SerializeField] GameObject optionScreen;

        void Start()
        {
            UIEventMaker.MakeButtonEvent(this);
        }

        public override void OnClicked()
        {
            UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);

            optionScreen.SetActive(true);
        }
    }
}

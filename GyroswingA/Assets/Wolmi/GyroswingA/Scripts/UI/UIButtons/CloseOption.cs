using UnityEngine;

namespace UIButtons
{
    public class CloseOption : UIButton
    {
        [SerializeField] GameCenter gameCenter;
        [SerializeField] InGameUIDisplay display;
        [SerializeField] GameObject optionScreen;

        void Start()
        {
            UIEventMaker.MakeButtonEvent(this);
        }

        public override void OnClicked()
        {
            UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnBack);

            if (SceneController.Instance.CurScene == SceneState.InGame)
            {
                display.SetInGameUI();
                gameCenter.MakeObjectsStartMoving();
            }
            else
            {
                optionScreen.SetActive(false);
            }
        }
    }
}
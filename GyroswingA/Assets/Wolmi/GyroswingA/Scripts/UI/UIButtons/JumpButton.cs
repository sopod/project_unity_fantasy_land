using UnityEngine;

namespace UIButtons
{
    public class JumpButton : UIButton
    {
        [SerializeField] Player player;

        void Start()
        {
            UIEventMaker.MakeButtonEvent(this);
        }

        public override void OnClicked()
        {
            player.MobileButtonAction(MoblieActionType.Jump);
        }
    }
}
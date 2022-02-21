using UnityEngine;

namespace UIButtons
{
    public class FireButton : UIButton
    {
        [SerializeField] Player player;

        void Start()
        {
            UIEventMaker.MakeButtonEvent(this);
        }

        public override void OnClicked()
        {
            player.MobileButtonAction(MoblieActionType.Fire);
        }
    }
}
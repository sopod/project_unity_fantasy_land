using UnityEngine;

namespace UIButtons
{
    public class DashButton : UIButton
    {
        [SerializeField] Player player;

        void Start()
        {
            UIEventMaker.MakeButtonEvent(this);
        }

        public override void OnClicked()
        {
            player.MobileButtonAction(MoblieActionType.Dash);
        }
    }
}
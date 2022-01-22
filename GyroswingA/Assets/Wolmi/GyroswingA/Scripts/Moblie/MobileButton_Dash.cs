using UnityEngine;

public class MobileButton_Dash : UIButton
{
    [SerializeField] PlayerController player;

    void Start()
    {
        UIEventMaker.MakeButtonEvent(this);
    }

    public override void OnClicked()
    {
        player.MobileButtonAction(MoblieActionType.Dash);
    }
}

using UnityEngine;

public class MobileButton_Fire : UIButton
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

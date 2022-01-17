using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileButton_Fire : UIButton
{
    [SerializeField] PlayerController player;
    void Start()
    {
        UIEventMaker.MakeButtonEvent(this.GetComponent<UIButton>());
    }
    public override void OnClicked()
    {
        player.MobileButtonAction(MoblieActionType.Fire);
    }
}

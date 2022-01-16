using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton_EasyModeScreen : UIButton
{
    [SerializeField] StageSelectionUIController ui;

    public override void OnClicked()
    {
        ui.SetEasyModeUI();
    }
}

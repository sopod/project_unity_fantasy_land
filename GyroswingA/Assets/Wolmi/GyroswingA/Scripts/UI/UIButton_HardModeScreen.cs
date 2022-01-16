using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton_HardModeScreen : UIButton
{
    [SerializeField] StageSelectionUIController ui;
    
    public override void OnClicked()
    {
        ui.SetHardModeUI();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton_StageSelectionScene : UIButton
{
    public override void OnClicked()
    {
        SceneLoader.LoadScene("StageSelection");
    }
}

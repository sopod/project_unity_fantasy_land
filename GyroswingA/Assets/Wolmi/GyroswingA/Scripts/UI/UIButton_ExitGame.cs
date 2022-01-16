using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton_ExitGame : UIButton
{

    public override void OnClicked()
    {
        Application.Quit();
    }
}

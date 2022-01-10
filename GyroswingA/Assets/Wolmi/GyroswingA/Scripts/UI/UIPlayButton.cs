using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayButton : UIButton
{
    public override void OnClicked()
    {
        SceneLoader.LoadScene("InGame");
    }
}
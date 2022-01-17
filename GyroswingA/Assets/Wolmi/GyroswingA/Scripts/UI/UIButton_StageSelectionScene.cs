using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton_StageSelectionScene : UIButton
{
    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);
        SceneLoader.LoadScene("StageSelection");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton_Option : UIButton
{
    [SerializeField] GameObject optionScreen;

    public override void OnClicked()
    {

        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);

        optionScreen.SetActive(true);
    }
}

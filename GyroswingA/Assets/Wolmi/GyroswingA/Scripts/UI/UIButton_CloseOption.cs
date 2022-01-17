using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton_CloseOption : UIButton
{
    [SerializeField] GameManager manager;
    [SerializeField] GameObject optionScreen;

    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnBack);

        optionScreen.SetActive(false);

        if (SceneManager.GetActiveScene().name == "InGame")
        {
            manager.SetStartMoving();
        }
    }
}

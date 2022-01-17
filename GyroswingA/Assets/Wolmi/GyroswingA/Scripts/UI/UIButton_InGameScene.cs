using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton_InGameScene : UIButton
{
    [SerializeField] StarDataPerLevel _dataPerLevel;

    GameMode modeForThisButton;
    int stageNumber;

    public void SetInGameButton(GameMode mode, int stageNumber)
    {
        modeForThisButton = mode;
        this.stageNumber = stageNumber;
    }

    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);

        _dataPerLevel.levelNumberCur = stageNumber;
        _dataPerLevel.stageModeCur = modeForThisButton;

        Invoke("StartInGame", 1.0f);
    }

    void StartInGame()
    {
        SceneLoader.LoadScene("InGame");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton_InGameScene : UIButton
{
    [SerializeField] StageData data;

    GameMode modeForThisButton;
    int stageNumber;

    public void SetInGameButton(GameMode mode, int stageNumber)
    {
        modeForThisButton = mode;
        this.stageNumber = stageNumber;
    }

    public override void OnClicked()
    {
        data.stageNumberCur = stageNumber;
        data.stageModeCur = modeForThisButton;

        SceneLoader.LoadScene("InGame");
    }
}
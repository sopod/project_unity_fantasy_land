using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    GameMode modeForThisButton;

    int stageNumber;

    [SerializeField] TextMeshProUGUI stageNumberText;
    [SerializeField] UIButton_InGameScene inGameButton;
    [SerializeField] StarDisplay starDisplay;
    [SerializeField] GameObject darkCover;

    public UIButton_InGameScene InGameButton { get { return inGameButton; } }

    public void SetStageButton(int stageNum, GameMode mode, int starsGot, bool setOn)
    {
        // set text
        modeForThisButton = mode;
        stageNumber = stageNum;
        stageNumberText.text = new StringBuilder("STAGE " + stageNum).ToString();


        // set stars and dark cover
        if (starsGot == 0 && !setOn)
        {
            darkCover.SetActive(true);
            starDisplay.TurnOff();
        }
        else
        {
            darkCover.SetActive(false);
            inGameButton.SetInGameButton(mode, stageNum);
            UIEventMaker.MakeButtonEvent(inGameButton);

            starDisplay.TurnOn(starsGot);
        }
    }
    

}

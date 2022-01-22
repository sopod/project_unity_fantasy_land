using System.Text;
using TMPro;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    GameMode modeForThisButton;

    int stageNumber;

    [SerializeField] TextMeshProUGUI stageNumberText;
    [SerializeField] UIButton_InGameScene inGameButton;
    [SerializeField] StarDisplay starDisplay;
    [SerializeField] GameObject darkCover;

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

            starDisplay.TurnOn(starsGot);
        }
    }
    

}

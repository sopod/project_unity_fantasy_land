using System.Text;
using TMPro;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    GameMode modeForThisButton;

    int stageNumber;

    [SerializeField] TextMeshProUGUI stageNumberText;
    [SerializeField] UIButtons.InGameScene inGameButton;
    [SerializeField] StarDisplay starDisplay;
    [SerializeField] GameObject darkCover;

    public void SetStageButton(int stageNum, GameMode mode, int starsGot, bool setOn)
    {
        modeForThisButton = mode;
        stageNumber = stageNum;
        stageNumberText.text = new StringBuilder("STAGE " + stageNum).ToString();

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButtonController : MonoBehaviour
{
    [SerializeField] StageData data;

    [SerializeField] GameMode modeForButtons;
    [SerializeField] StageButton[] buttons;
    
    
    public void SetButtons()
    {
        int curStars = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            curStars = data.GetStar(modeForButtons, i + 1);

            if (data.StageNumberUnlocked == (i + 1) && data.StageModeUnlocked == modeForButtons)
            {
                buttons[i].SetStageButton(i + 1, modeForButtons, curStars, true);
            }
            else
            {
                buttons[i].SetStageButton(i + 1, modeForButtons, curStars, false);
            }
        }
    }
    
}

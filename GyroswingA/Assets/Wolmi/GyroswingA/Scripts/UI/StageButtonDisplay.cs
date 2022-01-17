using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButtonDisplay : MonoBehaviour
{
    [SerializeField] StarDataPerLevel data;

    [SerializeField] GameMode modeForButtons;
    [SerializeField] StageButton[] buttons;
    
    
    public void SetButtons()
    {
        bool forceToOn = false;
        int curStars = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            forceToOn = false;

            if ((modeForButtons == GameMode.Easy && (i + 1) == data.UnlockedLevelMax_Easy) ||
                (modeForButtons == GameMode.Hard && (i + 1) == data.UnlockedLevelMax_Hard))
            {
               forceToOn = true;
            }

            curStars = data.GetStar(modeForButtons, i + 1);

            buttons[i].SetStageButton(i + 1, modeForButtons, curStars, forceToOn);
        }
    }
    
}

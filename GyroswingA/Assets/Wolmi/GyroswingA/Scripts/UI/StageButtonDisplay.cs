using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButtonDisplay : MonoBehaviour
{
    [SerializeField] StarDataPerLevel _dataPerLevel;

    [SerializeField] GameMode modeForButtons;
    [SerializeField] StageButton[] buttons;
    
    
    public void SetButtons()
    {
        int curStars = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            curStars = _dataPerLevel.GetStar(modeForButtons, i + 1);

            if (_dataPerLevel.LevelNumberUnlocked == (i + 1) && _dataPerLevel.LevelModeUnlocked == modeForButtons)
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

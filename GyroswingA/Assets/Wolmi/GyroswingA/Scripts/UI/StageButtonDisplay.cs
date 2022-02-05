using UnityEngine;

public class StageButtonDisplay : MonoBehaviour
{
    StarDataPerLevel data;

    [SerializeField] GameMode modeForButtons;
    [SerializeField] StageButton[] buttons;

    void Awake()
    {
        data = SceneController.Instance.loaderStarData.data;
    }
    
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

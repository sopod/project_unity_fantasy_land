using UnityEngine;



[CreateAssetMenu(fileName = "Star Data", menuName = "Gyroswing/Star Data")]
public class StarDataPerLevel : ScriptableObject
{
    const int requiredStarsToHardMode = 25; 
    const int levelCountPerMode = 10;
    public int LevelCountPerMode { get { return levelCountPerMode; } }

    public bool IsHardModeOpen { get { return TotalStar >= requiredStarsToHardMode; } }
    int TotalStar
    {
        get
        {
            int count = 0;

            for (int i = 0; i < levelCountPerMode; i++)
            {
                count += easyMode[i];
                count += hardMode[i];
            }

            return count;
        }
    }

    // to set In Game Scene at first
    public int levelNumberCur = 1;
    public GameMode stageModeCur = GameMode.Easy;



    [SerializeField] int unlockedLevelMax_Easy = 1;
    public int UnlockedLevelMax_Easy { get { return unlockedLevelMax_Easy; } }


    [SerializeField] int unlockedLevelMax_Hard = 0;
    public int UnlockedLevelMax_Hard { get { return unlockedLevelMax_Hard; } }


    [SerializeField] int[] easyMode = new int[levelCountPerMode];
    [SerializeField] int[] hardMode = new int[levelCountPerMode];

    



    [ContextMenu("Clear Data")]
    public void Clear()
    {
        levelNumberCur = 1;
        stageModeCur = GameMode.Easy;

        unlockedLevelMax_Easy = 1;
        unlockedLevelMax_Hard = 0;

        for (int i = 0; i < levelCountPerMode; i++)
        {
            easyMode[i] = 0;
            hardMode[i] = 0;
        }
    }

    public int GetStar(GameMode mode, int levelNum)
    {
        switch (mode)
        {
            case GameMode.Easy:
                return easyMode[levelNum - 1];
            case GameMode.Hard:
                return hardMode[levelNum - 1];
            default:
                return -1;
        }
    }

    public void SetStar(GameMode mode, int levelNum, int starCount)
    {
        switch (mode)
        {
            case GameMode.Easy:
            {
                //if (starCount > 0 && easyMode[levelNum - 1] == 0)
                //{
                //    SetUnlocked(mode, levelNum);
                //}
                if (starCount > easyMode[levelNum - 1])
                {
                    easyMode[levelNum - 1] = starCount;
                }
            }
                break;

            case GameMode.Hard:
            {
                //if (starCount > 0 && hardMode[levelNum - 1] == 0)
                //{
                //    SetUnlocked(mode, levelNum);
                //}
                if (starCount > hardMode[levelNum - 1])
                {
                    hardMode[levelNum - 1] = starCount;
                }
            }
                break;

            default:
            {
            }
                break;
        }
    }

    public void SetUnlocked(GameMode mode, int levelNum)
    {
        if (mode == GameMode.Easy)
        {
            if (levelNum <= unlockedLevelMax_Easy) return;
            unlockedLevelMax_Easy = levelNum;
        }
        else
        {
            if (levelNum <= unlockedLevelMax_Hard) return;
            unlockedLevelMax_Hard = levelNum;
        }

        if (unlockedLevelMax_Hard == 0 && IsHardModeOpen)
        {
            unlockedLevelMax_Hard = 1;
        }
    }
}

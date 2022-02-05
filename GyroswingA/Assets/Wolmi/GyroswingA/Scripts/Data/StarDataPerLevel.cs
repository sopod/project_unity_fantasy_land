using UnityEngine;


[System.Serializable]
public class StarDataPerLevel
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

    public void UnlockAll()
    {
        levelNumberCur = 1;
        stageModeCur = GameMode.Easy;

        unlockedLevelMax_Easy = 10;
        unlockedLevelMax_Hard = 10;

        for (int i = 0; i < levelCountPerMode; i++)
        {
            easyMode[i] = 3;
            hardMode[i] = 3;
        }
    }

    public int GetStar(GameMode mode, int levelNum)
    {
        switch (mode)
        {
            case GameMode.Easy: return easyMode[levelNum - 1];
            case GameMode.Hard: return hardMode[levelNum - 1];
        }
        return -1;
    }

    public void SetStar(GameMode mode, int levelNum, int starCount)
    {
        if (mode == GameMode.Easy && starCount > easyMode[levelNum - 1])
            easyMode[levelNum - 1] = starCount;
        else if (mode == GameMode.Hard && starCount > hardMode[levelNum - 1])
            hardMode[levelNum - 1] = starCount;
    }

    public void SetUnlocked(GameMode mode, int levelNum)
    {
        if (mode == GameMode.Easy && levelNum > unlockedLevelMax_Easy)
            unlockedLevelMax_Easy = levelNum;
        else if (mode == GameMode.Hard && levelNum > unlockedLevelMax_Hard)
            unlockedLevelMax_Hard = levelNum;

        if (unlockedLevelMax_Hard == 0 && IsHardModeOpen)
            unlockedLevelMax_Hard = 1;
    }
}


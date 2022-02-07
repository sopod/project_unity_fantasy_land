using UnityEngine;



[System.Serializable]
public class LevelChanger
{
    int levelCur = 0;
    public int LevelCur { get => levelCur; }
    GameMode modeCur = GameMode.Easy;
    public GameMode ModeCur { get => modeCur; }

    GoogleSheetDataLoader levelValues;
    StarDataLoader starData;

    public void Init(GoogleSheetDataLoader levelValues, StarDataLoader starData)
    {
        this.levelValues = levelValues;
        this.starData = starData;

        SetLevel(starData.data.stageModeCur, starData.data.levelNumberCur);
    }

    void SetLevel(GameMode mode, int level)
    {
        modeCur = mode;
        levelCur = level;
    }

    public bool UpgradeLevel() // after get star
    {
        levelCur++;

        if (levelCur > starData.data.LevelCountPerMode && modeCur == GameMode.Easy)
        {
            if (!starData.data.IsHardModeOpen)
            {
                return false;
            }

            levelCur = 1;
            modeCur = GameMode.Hard;
        }
        else if (levelCur > starData.data.LevelCountPerMode && modeCur == GameMode.Hard)
        {
            return false;
        }


        SetLevel(modeCur, levelCur);
        starData.data.SetUnlocked(modeCur, levelCur);

        starData.SaveStarData();

        return true;
    }

    public LevelValues GetCurLevelValues()
    {
        return levelValues.GetLevelValueCur(modeCur, levelCur);
    }

    public int GetMonsterAmountForCurState()
    {
        return (modeCur == GameMode.Easy ? 5 : 9);
    }

    public float GetEnemyMoveSpeed(EnemyType type, float playerMoveSpeed)
    {
        switch (type)
        {
            case EnemyType.Juck:
            case EnemyType.Swook: return playerMoveSpeed * 1.3f;
            case EnemyType.Gum: return playerMoveSpeed * 1.2f;
        }
        return playerMoveSpeed;
    }

    public float GetDashPowerToDamaged(EnemyType type, float dashPowerToDamaged)
    {
        switch (type)
        {
            case EnemyType.Juck:
            case EnemyType.Swook: return dashPowerToDamaged * 1.2f;
        }
        return dashPowerToDamaged;
    }

    public float GetItemSecondsToAdd(ItemType type)
    {
        switch (type)
        {
            case ItemType.HarippoBlue:return 5.0f;
            case ItemType.HarippoGreen: return 7.0f;
            case ItemType.HarippoYellow: return 10.0f;
            case ItemType.HarippoRed: return 20.0f;
        }
        return 0.0f;
    }

    public void OnPlayerSpeedItemUsed(ItemType type, float playerMoveSpeed)
    {
        switch (type)
        {
            case ItemType.Coke: playerMoveSpeed *= 1.1f; break;
            case ItemType.ChocoTarte: playerMoveSpeed *= 1.3f; break;
        }
    }
}

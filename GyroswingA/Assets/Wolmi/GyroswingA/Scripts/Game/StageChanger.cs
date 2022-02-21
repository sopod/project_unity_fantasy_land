

[System.Serializable]
public class StageChanger
{
    int stageCur = 0;
    public int StageCur { get => stageCur; }

    GameMode modeCur = GameMode.Easy;
    public GameMode ModeCur { get => modeCur; }

    ObjectValues values;
    GoogleSheetDataLoader googleDataLoader;
    StarDataLoader starDataLoader;

    
    public StageChanger(GoogleSheetDataLoader googleDataLoader, StarDataLoader starDataLoader)
    {
        this.googleDataLoader = googleDataLoader;
        this.values = googleDataLoader.ObjectDatas;
        this.starDataLoader = starDataLoader;

        SetStage(starDataLoader.data.stageModeCur, starDataLoader.data.levelNumberCur);
    }

    void SetStage(GameMode mode, int level)
    {
        modeCur = mode;
        stageCur = level;
    }

    public bool UpgradeStage()
    {
        stageCur++;

        if (stageCur > starDataLoader.data.LevelCountPerMode)
        {
            if (modeCur == GameMode.Hard || !starDataLoader.data.IsHardModeOpen) return false;
            
            stageCur = 1;
            modeCur = GameMode.Hard;
        }

        SetStage(modeCur, stageCur);

        starDataLoader.data.SetUnlocked(modeCur, stageCur);
        starDataLoader.SaveStarData();

        return true;
    }

    public LevelValues GetCurrentStageValue()
    {
        return googleDataLoader.GetCurrentStageValue(modeCur, stageCur);
    }

    public int GetMonsterMaxForCurrentStage()
    {
        return (modeCur == GameMode.Easy ? values.EasyModeMonsterMax : values.HardModeMonsterMax);
    }

}

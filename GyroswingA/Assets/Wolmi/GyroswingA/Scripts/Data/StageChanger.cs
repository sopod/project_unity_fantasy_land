

// 게임의 스테이지을 변경하는 클래스를 따로 만들어 관리하였습니다. 


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

    // 현재 스테이지를 다음 스테이지로 업그레이드합니다. 
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

        // 모은 별 데이터를 토대로 스테이지를 설정하고, 별 데이터를 저장합니다. 
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

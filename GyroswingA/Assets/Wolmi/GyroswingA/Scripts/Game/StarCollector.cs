public class StarCollector
{
    int starCur = 0;
    public int GetStarCur { get => starCur; }

    int GetStarForCurStage(GameMode mode, int remainingMonsterCur)
    {
        int got = 0;

        switch (mode)
        {
            case GameMode.Easy:
            {
                if (remainingMonsterCur <= 1) got = 3;
                else if (remainingMonsterCur <= 3) got = 2;
                else if (remainingMonsterCur <= 4) got = 1;
                else got = 0;
            }
            break;

            case GameMode.Hard:
            {
                if (remainingMonsterCur <= 1) got = 3;
                else if (remainingMonsterCur <= 5) got = 2;
                else if (remainingMonsterCur <= 7) got = 1;
                else got = 0;
            }
            break;
        }

        return got;
    }

    public bool SetStar(StarDataLoader starData, GameMode mode, int levelNum, int remainingMonster)
    {
        starCur = GetStarForCurStage(mode, remainingMonster);
        starData.data.SetStar(mode, levelNum, starCur);

        return (starCur != 0);
    }
}

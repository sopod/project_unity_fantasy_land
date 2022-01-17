using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;



[CreateAssetMenu(fileName = "Star Data", menuName = "Gyroswing/Star Data")]
public class StarDataPerLevel : ScriptableObject
{
    const int levelCountPerMode = 10;
    public int LevelCountPerMode { get { return levelCountPerMode; } }


    // to set In Game Scene at first
    public int levelNumberCur = 1;
    public GameMode stageModeCur = GameMode.Easy;



    [SerializeField] int levelNumberUnlocked = 1;
    [SerializeField] GameMode levelModeUnlocked = GameMode.Easy;
    public int LevelNumberUnlocked { get { return levelNumberUnlocked; } }
    public GameMode LevelModeUnlocked { get { return levelModeUnlocked; } }


    [SerializeField] int[] easyMode = new int[levelCountPerMode];
    [SerializeField] int[] hardMode = new int[levelCountPerMode];

    



    [ContextMenu("Clear Data")]
    public void Clear()
    {
        levelNumberCur = 1;
        stageModeCur = GameMode.Easy;

        levelNumberUnlocked = 1;
        levelModeUnlocked = GameMode.Easy;

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
        levelModeUnlocked = mode;
        levelNumberUnlocked = levelNum;

        //if (mode == GameMode.Easy && levelNum + 1 > 10)
        //{
        //    levelModeUnlocked = GameMode.Hard;
        //    levelNumberUnlocked = 1;
        //}
        //else if (mode == GameMode.Hard && levelNum + 1 > 10)
        //{
        //    levelModeUnlocked = GameMode.Hard;
        //    levelNumberUnlocked = 10;
        //}
        //else
        //{
        //    levelNumberUnlocked = levelNum + 1;
        //}
    }
}

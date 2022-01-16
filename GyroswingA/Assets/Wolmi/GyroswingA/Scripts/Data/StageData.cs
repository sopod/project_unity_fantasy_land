using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;



[CreateAssetMenu(fileName = "Stage Data", menuName = "Gyroswing/Stage Data")]
public class StageData : ScriptableObject
{
    const int stageCountPerMode = 10;

    public int StageCountPerMode
    {
        get { return stageCountPerMode; }
    }

    // to set In Game Scene at first
    public int stageNumberCur = 1;
    public GameMode stageModeCur = GameMode.Easy;

    [SerializeField] int stageNumberUnlocked = 1;
    [SerializeField] GameMode stageModeUnlocked = GameMode.Easy;

    public int StageNumberUnlocked
    {
        get { return stageNumberUnlocked; }
    }

    public GameMode StageModeUnlocked
    {
        get { return stageModeUnlocked; }
    }


    [SerializeField] int[] easyMode = new int[stageCountPerMode];
    [SerializeField] int[] hardMode = new int[stageCountPerMode];


    public void Clear()
    {
        stageNumberCur = 1;
        stageModeCur = GameMode.Easy;

        stageNumberUnlocked = 1;
        stageModeUnlocked = GameMode.Easy;

        for (int i = 0; i < stageCountPerMode; i++)
        {
            easyMode[i] = 0;
            hardMode[i] = 0;
        }
    }

    public int GetStar(GameMode mode, int stageNumber)
    {
        switch (mode)
        {
            case GameMode.Easy:
                return easyMode[stageNumber - 1];
            case GameMode.Hard:
                return hardMode[stageNumber - 1];
            default:
                return -1;
        }
    }

    public void SetStar(GameMode mode, int stageNumber, int starCount)
    {
        switch (mode)
        {
            case GameMode.Easy:
            {
                //if (starCount > 0 && easyMode[stageNumber - 1] == 0)
                //{
                //    SetUnlocked(mode, stageNumber);
                //}
                if (starCount > easyMode[stageNumber - 1])
                {
                    easyMode[stageNumber - 1] = starCount;
                }
            }
                break;

            case GameMode.Hard:
            {
                //if (starCount > 0 && hardMode[stageNumber - 1] == 0)
                //{
                //    SetUnlocked(mode, stageNumber);
                //}
                if (starCount > hardMode[stageNumber - 1])
                {
                    hardMode[stageNumber - 1] = starCount;
                }
            }
                break;

            default:
            {
            }
                break;
        }
    }

    public void SetUnlocked(GameMode mode, int stageNumber)
    {
        stageModeUnlocked = mode;
        stageNumberUnlocked = stageNumber;

        //if (mode == GameMode.Easy && stageNumber + 1 > 10)
        //{
        //    stageModeUnlocked = GameMode.Hard;
        //    stageNumberUnlocked = 1;
        //}
        //else if (mode == GameMode.Hard && stageNumber + 1 > 10)
        //{
        //    stageModeUnlocked = GameMode.Hard;
        //    stageNumberUnlocked = 10;
        //}
        //else
        //{
        //    stageNumberUnlocked = stageNumber + 1;
        //}
    }
}

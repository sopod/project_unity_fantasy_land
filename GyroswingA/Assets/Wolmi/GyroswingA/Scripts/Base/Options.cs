using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class Options
{
    int levelCur = 0;

    // game 
    [HideInInspector] public int EnemyPrepareAmount = 5;
    [HideInInspector] public int LimitSecondsPerStage = 180;
    [HideInInspector] public int RequiredStarAmountForHardMode = 25;
    [HideInInspector] public int StageAmountPerMode = 10;

    // machine
    [HideInInspector] public bool IsMachineSwinging = true;
    [HideInInspector] public bool IsMachineTurning = true;
    [HideInInspector] public bool IsMachineSpining = true;
    
    [SerializeField] LevelValues[] LevelValues;
    [HideInInspector] public bool IsSpiningCW = true;


    // player
    [HideInInspector] public float Gravity = 9.8f;
    public float PlayerMoveSpeed = 2.0f;
    public float PlayerRotateSpeed = 40.0f;
    public float PlayerJumpPower = 5.0f;
    public float PlayerDashPower = 1000.0f;
    public float WaitTimeAfterDash = 0.5f;

    // monster



    public LevelValues GetCurLevelValues()
    {
        return LevelValues[levelCur];
    }

    public void SetLevel(int level)
    {
        levelCur = level;
    }

    public int GetMonsterAmountForCurState(GameMode mode)
    {
        return (mode == GameMode.Easy ? 5 : 9);
    }

    public int GetStarAmountForCurStage(GameMode mode, int remainingMonsterCur)
    {
        switch (mode)
        {
            case GameMode.Easy:
            {
                if (remainingMonsterCur <= 1)
                    return 3;
                else if (remainingMonsterCur <= 3)
                    return 2;
                else if (remainingMonsterCur <= 4)
                    return 1;
                else
                    return 0;
            }

            case GameMode.Hard:
            {
                if (remainingMonsterCur <= 1)
                    return 3;
                else if (remainingMonsterCur <= 5)
                    return 2;
                else if (remainingMonsterCur <= 7)
                    return 1;
                else
                    return 0;
            }
        }

        return 0;
    }

}

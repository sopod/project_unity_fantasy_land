using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


// 에디터에서 설정하기.

[System.Serializable]
public class Options
{
    // layer
    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;
    public LayerMask StageLayer;
    public LayerMask FailZoneLayer;
    //public LayerMask platformLayer;
    public LayerMask StagePoleLayer;
    public LayerMask StageBoundaryLayer;



    // level
    int _levelCur = 0;
    GameMode _modeCur;
    [SerializeField] LevelValues LevelValues;



    // game 
    [HideInInspector] public int EnemyPrepareAmount = 5;
    [HideInInspector] public int LimitSecondsPerStage = 180;
    [HideInInspector] public int RequiredStarAmountForHardMode = 25;
    [HideInInspector] public int StageAmountPerMode = 10;



    // machine
    [HideInInspector] public bool IsMachineSwinging = true;
    [HideInInspector] public bool IsMachineTurning = true;
    [HideInInspector] public bool IsMachineSpining = true;
    [HideInInspector] public bool IsSpiningCW = true;



    // player
    [HideInInspector] public Vector3 PlayerStartPos = new Vector3(-32.0f, 2.2f, -50.2f);
    public float PlayerMoveSpeed = 2.0f;
    public float PlayerRotateSpeed = 40.0f;
    public float PlayerJumpPower = 3.0f;



    // common
    [HideInInspector] public float Gravity = 9.8f;



    // skill
    public float DashPowerToHit = 20.0f;
    public float DashPowerToDamaged = 10.0f;
    [HideInInspector] public float WaitForAnotherDash = 0.5f;



    // enemy
    public float EnemyMoveSpeed = 2.0f;
    public float EnemyRotateSpeed = 40.0f;
    public float EnemyJumpPower = 3.0f;

    public float EnemyKnockDownTime = 2.0f;




    public LevelValues GetCurLevelValues()
    {
        return LevelValues;
    }

    public void ChangeLevel(GameMode mode, int level)
    {
        _modeCur = mode;
        _levelCur = level;
        LevelValues.ChangeLevel(mode, level);
    }

    public int GetMonsterAmountForCurState()
    {
        return (_modeCur == GameMode.Easy ? 5 : 9);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


// 에디터에서 설정하기.

[System.Serializable]
public class Options
{
    // level
    int _levelCur = 0;
    GameMode _modeCur;


    [Header("---- level")]
    [SerializeField] LevelValues LevelValues;

    [Header("---- game")]
    public int LimitSecondsPerStage;

    [HideInInspector] public float ResultSoundWaitingTime;
    [HideInInspector] public float ResultUIWaitingTime;

    [HideInInspector] public float ResultUIRemainingTime;
    [HideInInspector] public float GameStartWaitingTime;
    [HideInInspector] public float EnemyStartWaitingTime;

    [HideInInspector] public int SpawnerPrepareAmount;
    [HideInInspector] public int RequiredStarAmountForHardMode;
    [HideInInspector] public int StageAmountPerMode;


    [Header("---- machine")]
    public bool IsMachineSwinging;
    public bool IsMachineTurning;
    public bool IsMachineSpining;

    [HideInInspector] public Vector3 StageStartPos;
    [HideInInspector] public Quaternion StageStartRot;
    [HideInInspector] public Vector3 BarStartPos;
    [HideInInspector] public Quaternion BarStartRot;
    [HideInInspector] public bool IsSpiningCW;


    [Header("---- common")]
    [HideInInspector] public float Gravity;


    [Header("---- player")]
    public float PlayerMoveSpeed;
    public float PlayerRotateSpeed;
    public float PlayerJumpPower;
    [HideInInspector] public Vector3 PlayerStartPos;
    [HideInInspector] public Quaternion PlayerStartRot;

    [Header("---- enemy")]
    public float EnemyRotateSpeed;
    public float EnemyJumpPower;
    public float EnemyKnockDownTime;

    [Header("---- skill")]
    public float SkillCoolTime;
    public float DashPowerToDamaged;
    [HideInInspector] public float DashPowerToHit;
    public float FireBallPowerToDamaged;
    public float ProjectileMoveSpeed;
    public float ProjectileRemaingTime;

    [Header("---- layers")]
    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;
    public LayerMask ItemLayer;
    public LayerMask StageLayer;
    public LayerMask FailZoneLayer;
    public LayerMask StagePoleLayer;
    public LayerMask StageBoundaryLayer;
    public LayerMask ShootProjectileLayer;





    public void ResetOptionValuesByCode()
    {
        // game 
        ResultSoundWaitingTime = 0.3f;
        ResultUIWaitingTime = 0.4f;
        ResultUIRemainingTime = 5.0f;

        GameStartWaitingTime = 6.0f;
        EnemyStartWaitingTime = 1.0f;
        
        SpawnerPrepareAmount = 5;
        LimitSecondsPerStage = 20; // Fix this !!!!!!!!!!!!!!!!!!! 180 
        RequiredStarAmountForHardMode = 25;
        StageAmountPerMode = 10;

        IsMachineSwinging = true;
        IsMachineTurning = true;
        IsMachineSpining = true;

        IsSpiningCW = true;

        // common
        Gravity = 9.8f;

        // player
        PlayerStartPos = new Vector3(-32.0f, 2.2f, -50.2f);
        PlayerMoveSpeed = 2.0f; // Fix this
        PlayerRotateSpeed = 40.0f; // Fix this
        PlayerJumpPower = 6.0f; // Fix this

        // enemy
        EnemyRotateSpeed = 40.0f; // Fix this
        EnemyJumpPower = 6.0f; // Fix this
        EnemyKnockDownTime = 2.0f; // Fix this

        // skill
        DashPowerToHit = 20.0f;
        DashPowerToDamaged = 10.0f; // Fix this
        FireBallPowerToDamaged = 20.0f;
        SkillCoolTime = 0.5f; 
        ProjectileMoveSpeed = 7.0f;
        ProjectileRemaingTime = 1.6f;
    }                       

    public void ResetOptions()
    {
        PlayerMoveSpeed = 2.0f;
    }   

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

    public float GetEnemyMoveSpeed(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Juck:
            case EnemyType.Swook:
            {
                return PlayerMoveSpeed * 1.3f;
            }
            case EnemyType.Gum:
            {
                return PlayerMoveSpeed * 1.2f;
            }
            default:
            {
                return PlayerMoveSpeed;
            }
        }
    }

    public float GetDashPowerToDamaged(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Juck:
            case EnemyType.Swook:
            {
                return DashPowerToDamaged * 1.2f;
            }
            default:
            {
                return DashPowerToDamaged;
            }
        }
    }

    public float GetItemSecondsToAdd(ItemType type)
    {
        switch (type)
        {
            case ItemType.HarippoBlue:
            {
                return 5.0f;
            }
            case ItemType.HarippoGreen:
            {
                return 7.0f;
            }
            case ItemType.HarippoYellow:
            {
                return 10.0f;
            }
            case ItemType.HarippoRed:
            {
                return 20.0f;
            }
            default:
            {
                return 0.0f;
            }
        }
    }

    public void OnPlayerSpeedItemUsed(ItemType type)
    {
        switch (type)
        {
            case ItemType.Coke:
            {
                PlayerMoveSpeed *= 1.1f;
            }
                break;
            case ItemType.ChocoTarte:
            {
                PlayerMoveSpeed *= 1.3f;
            }
                break;
            default:
            {
            }
                break;
        }
    }
}

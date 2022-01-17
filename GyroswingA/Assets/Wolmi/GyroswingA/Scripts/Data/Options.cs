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
    public LayerMask ItemLayer;
    public LayerMask StageLayer;
    public LayerMask FailZoneLayer;
    public LayerMask StagePoleLayer;
    public LayerMask StageBoundaryLayer;
    
    // level
    int _levelCur = 0;
    GameMode _modeCur;
    [SerializeField] LevelValues LevelValues;

    // game 
    public float ResultUIShowingTime = 3.0f;
    public float GameStartWaitingTime = 4.0f;
    public int EnemyPrepareAmount = 5;
    public int LimitSecondsPerStage = 180;
    public int RequiredStarAmountForHardMode = 25;
    public int StageAmountPerMode = 10;

    // machine
    public Vector3 StageStartPos;
    public Quaternion StageStartRot;
    public Vector3 BarStartPos;
    public Quaternion BarStartRot;

    public bool IsMachineSwinging = true;
    public bool IsMachineTurning = true;
    public bool IsMachineSpining = true;
    public bool IsSpiningCW = true;

    // common
    public float Gravity = 9.8f;
    
    // player
    public Vector3 PlayerStartPos = new Vector3(-32.0f, 2.2f, -50.2f);
    public Quaternion PlayerStartRot;
    public float PlayerMoveSpeed = 2.0f;
    public float PlayerRotateSpeed = 40.0f;
    public float PlayerJumpPower = 3.0f;
    
    // enemy
    public float EnemyRotateSpeed = 40.0f;
    public float EnemyJumpPower = 3.0f;
    public float EnemyKnockDownTime = 2.0f;
    
    // skill
    public float DashPowerToHit = 20.0f;
    public float DashPowerToDamaged = 10.0f;
    public float SkillCoolTime = 0.5f;


    public void ResetOptions()
    {
        // game 
        ResultUIShowingTime = 3.0f;
        GameStartWaitingTime = 4.0f;
        EnemyPrepareAmount = 5;
        LimitSecondsPerStage = 180;
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
        PlayerMoveSpeed = 2.0f;
        PlayerRotateSpeed = 40.0f;
        PlayerJumpPower = 3.0f;

        // enemy
        EnemyRotateSpeed = 40.0f;
        EnemyJumpPower = 3.0f;
        EnemyKnockDownTime = 2.0f;

        // skill
        DashPowerToHit = 20.0f;
        DashPowerToDamaged = 10.0f;
        SkillCoolTime = 0.5f;
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

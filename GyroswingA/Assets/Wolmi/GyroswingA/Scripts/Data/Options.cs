using System;
using UnityEngine;



[System.Serializable]
public class Options : MonoBehaviour
{
    // level
    int _levelCur = 0;
    GameMode _modeCur;


    [Header("---- level")]
    GoogleSheetDataLoader Values;

    [HideInInspector] public float WaitingTimeForCinemachine;
    [HideInInspector] public int LimitSecondsPerStage;

    [HideInInspector] public float ResultSoundWaitingTime;
    [HideInInspector] public float ResultUIWaitingTime;

    [HideInInspector] public float ResultUIRemainingTime;
    [HideInInspector] public float GameStartWaitingTime;
    [HideInInspector] public float EnemyStartWaitingTime;

    [HideInInspector] public int SpawnerPrepareAmount;
    [HideInInspector] public int StageAmountPerMode;


    [HideInInspector] public bool IsMachineSwinging;
    [HideInInspector] public bool IsMachineTurning;
    [HideInInspector] public bool IsMachineSpining;

    [HideInInspector] public Vector3 StageStartPos;
    [HideInInspector] public Quaternion StageStartRot;
    [HideInInspector] public Vector3 BarStartPos;
    [HideInInspector] public Quaternion BarStartRot;
    [HideInInspector] public bool IsSpiningCW;


    [Header("---- common")]
    [HideInInspector] public float Gravity;


    [Header("---- player")]
    public float PlayerMoveSpeed = 2.0f;
    public float PlayerRotateSpeed = 150.0f;
    public float PlayerJumpPower = 5.0f;
    [HideInInspector] public Vector3 PlayerStartPos;
    [HideInInspector] public Quaternion PlayerStartRot;

    [Header("---- enemy")]
    public float EnemyRotateSpeed = 150.0f;
    public float EnemyJumpPower = 5.0f;
    [HideInInspector] public float EnemyKnockDownTime = 2.0f;

    [Header("---- skill")] 
    public ProjectileSpawner ProjectilesSpawner;
    [HideInInspector] public float SkillCoolTime;
    public float DashPowerToDamaged = 10.0f;
    [HideInInspector] public float DashPowerToHit;
    public float FireBallPowerToDamaged = 20.0f;
    public float ProjectileMoveSpeed = 7.0f;
    public float ProjectileRemaingTime = 1.6f;

    [Header("---- layers")]
    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;
    public LayerMask ItemLayer;
    public LayerMask StageLayer;
    public LayerMask FailZoneLayer;
    public LayerMask StageBoundaryLayer;
    public LayerMask ShootProjectileLayer;


    void Start()
    {
        Values = FindObjectOfType<GoogleSheetDataLoader>();
    }


    public void ResetOptionValuesByCode()
    {
        // game 
        WaitingTimeForCinemachine = 8.0f;
        ResultSoundWaitingTime = 0.3f;
        ResultUIWaitingTime = 0.4f;
        ResultUIRemainingTime = 5.0f;

        GameStartWaitingTime = 5.5f;
        EnemyStartWaitingTime = 1.0f;
        
        SpawnerPrepareAmount = 5;
        LimitSecondsPerStage = 180;
        StageAmountPerMode = 10;

        IsMachineSwinging = true;
        IsMachineTurning = true;
        IsMachineSpining = true;

        IsSpiningCW = true;

        // common
        Gravity = 9.8f;

        // player
        PlayerStartPos = new Vector3(-30.57f, 2.21f, -50.2f);
        //PlayerMoveSpeed = 2.0f;
        //PlayerRotateSpeed = 150.0f;
        //PlayerJumpPower = 5.0f;

        // enemy
        //EnemyRotateSpeed = 150.0f;
        //EnemyJumpPower = 5.0f;
        EnemyKnockDownTime = 2.0f;

        // skill
        DashPowerToHit = 20.0f;
        //DashPowerToDamaged = 10.0f;
        //FireBallPowerToDamaged = 20.0f;
        SkillCoolTime = 0.5f; 
        //ProjectileMoveSpeed = 7.0f;
        //ProjectileRemaingTime = 1.6f;
    }                       

    public void ResetOptions()
    {
        PlayerMoveSpeed = 2.0f;
    }   

    public LevelValues GetCurLevelValues()
    {
        return Values.GetLevelValueCur(_modeCur, _levelCur);
    }

    public void ChangeLevel(GameMode mode, int level)
    {
        _modeCur = mode;
        _levelCur = level;
        Values.GetLevelValueCur(mode, level);
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
            case EnemyType.Swook: return PlayerMoveSpeed * 1.3f;
            case EnemyType.Gum: return PlayerMoveSpeed * 1.2f;
        }
        return PlayerMoveSpeed;
    }

    public float GetDashPowerToDamaged(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Juck:
            case EnemyType.Swook: return DashPowerToDamaged * 1.2f;
        }
        return DashPowerToDamaged;
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

    public void OnPlayerSpeedItemUsed(ItemType type)
    {
        switch (type)
        {
            case ItemType.Coke: PlayerMoveSpeed *= 1.1f; break;
            case ItemType.ChocoTarte:PlayerMoveSpeed *= 1.3f; break;
        }
    }
}

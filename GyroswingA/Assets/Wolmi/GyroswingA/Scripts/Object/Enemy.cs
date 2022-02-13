using System.Collections.Generic;
using UnityEngine;

public enum EnemyMovement
{
    Wait,
    MoveForward,
    MoveBackward,
    TurnRight,
    TurnLeft,
    JumpForward,
    Max
}

public class MovementData
{
    public EnemyMovement movement;
    public Node btNode;
    public float time;

    public MovementData(EnemyMovement movement, Node btNode, float time)
    {
        this.movement = movement;
        this.btNode = btNode;
        this.time = time;
    }
}

public class Enemy : LivingCreature, ISpawnableObject
{
    BT_Dragon bt;
    Queue<MovementData> movementDatas;
    public bool movementsAllDone { get { return movementDatas.Count == 0; } }
    StopWatch movementTimer;

    [SerializeField] EnemyType enemyType;
    public int Type 
    { 
        get => (int) enemyType;
        set { enemyType = (EnemyType) value; }
    }

    bool checkEnemyToMove;
    bool isKnockDown;
    public bool IsEnemySet = false;


    void Update()
    {
        if (IsPaused) return;
        if (IsStopped) return;
        
        DoMovement();
        CheckMovementIsFinished();
        CheckKnockDownIsFinished();
        SetIsMovingAnimation();

        if (movementsAllDone && !isKnockDown)
            bt.UpdateBT();
    }

    void FixedUpdate()
    {
        if (IsPaused) return;

        AffectedByPhysics();
    }

    public void SetEnemy(GameObject stage, StageMovementValue stageVal, LevelChanger options, Layers layer, ProjectileSpawner pjSpanwer)
    {
        values = SceneController.Instance.loaderGoogleSheet.ObjectDatas;

        bt = GetComponent<BT_Dragon>();

        creatureType = CreatureType.Enemy;
        movementDatas = new Queue<MovementData>();
        movementTimer = new StopWatch();

        curMoveSpeed = options.GetEnemyMoveSpeed(enemyType, values.MoveSpeed);

        checkEnemyToMove = false;
        isKnockDown = false;

        SetCreature(stage, stageVal, options, layer, pjSpanwer);
    }

    public override void StartMoving()
    {
        base.StartMoving();
        
        bt.SetBT(layerStruct);
    }
    
    public void AddEnemyMovement(MovementData input)
    {
        movementDatas.Enqueue(input);

        if (movementTimer.IsRunning) return;

        StartMovement();
    }

    void StartMovement()
    {
        checkEnemyToMove = true;
        movementTimer.StartTimer(movementDatas.Peek().time);

        if (movementDatas.Peek().movement == EnemyMovement.JumpForward)
            Jump();
    }

    void DoMovement()
    {
        if (!checkEnemyToMove || movementsAllDone) return;

        switch (movementDatas.Peek().movement)
        {
            case EnemyMovement.Wait: MakeEnemyDoIdleAnimation(); break;
            case EnemyMovement.MoveForward: Move(1.0f); break;
            case EnemyMovement.MoveBackward: Move(-1.0f); break;
            case EnemyMovement.TurnRight: Turn(1.0f); break;
            case EnemyMovement.TurnLeft: Turn(-1.0f); break;
            case EnemyMovement.JumpForward: if (isJumping) Move(1.0f); break;
            default: break;
        }
    }

    void CheckMovementIsFinished()
    {
        if (!checkEnemyToMove || !movementTimer.IsFinished) return;

        if (movementDatas.Peek().btNode != null)
            movementDatas.Peek().btNode.SetFinishedFlag(true);

        movementDatas.Dequeue();
        movementTimer.FinishTimer();
        checkEnemyToMove = false;

        if (!movementsAllDone)
            StartMovement();
    }

    void DeleteAllMovementsAndStop()
    {
        movementTimer.FinishTimer();

        for (int i = 0; i < movementDatas.Count; i++)
        {
            if (movementDatas.Peek().btNode != null)
                movementDatas.Peek().btNode.SetFailureState();
            movementDatas.Dequeue();
        }
        
        checkEnemyToMove = false;
        InitAnimation();
    }

    void CheckKnockDownIsFinished()
    {
        if (!isKnockDown || !movementTimer.IsFinished) return;
        
        movementTimer.FinishTimer();
        isKnockDown = false;
    }

    public void AttackPlayer()
    {
        Dash();
    }

    void MakeEnemyDoIdleAnimation()
    {
        isMoving = false; 
        isTurning = false;
    }

    protected override void NotifyDead()
    {
        soundPlayer.PlaySound(CreatureEffectSoundType.Dead, true);
        GameCenter.Instance.OnMonsterKilled();
    }

    // -------------------------------------------------- damaged by player fire
    void OnTriggerEnter(Collider other)
    {
        if (IsPaused) return;

        int layer = (1 << other.gameObject.layer);

        if (layer != layerStruct.ShootProjectileLayer.value) return;

        GameObject p = pjSpanwer.SpawnFireHitProjectile(this.gameObject);
        p.GetComponent<Projectile>().SetStart(pjSpanwer);
        OnDamagedAndMoveBack(true, other.transform.position, other.transform.forward, EnemyType.Max);
    }

    // -------------------------------------------------- damaged by player dash
    void OnCollisionStay(Collision collision)
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);
        if (layer != layerStruct.PlayerLayer.value) return;

        LivingCreature l = collision.gameObject.GetComponent<LivingCreature>();
        if (!l.IsAttacking || isDamaged) return;

        GameObject p = pjSpanwer.SpawnDashHitProjectile(collision);
        p.GetComponent<Projectile>().SetStart(pjSpanwer);
        OnDamagedAndMoveBack(false, l.CenterPosition, l.CenterForward, EnemyType.Max);
    }

    // -------------------------------------------------- layer collision
    void OnCollisionEnter(Collision collision)
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);

        if (layer == layerStruct.StageLayer.value)
        {
            OnStageLayer();
        }
        else if (layer == layerStruct.FailZoneLayer.value)
        {
            OnFailZoneLayer();
        }
        else if (layer == layerStruct.EnemyLayer.value)
        {
            OnEnemyLayer();
        }
        else if (layer == layerStruct.PlayerLayer.value)
        {
            OnPlayerLayer();
        }
        else
        {
            OnNothingLayer();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);
        if (layer != layerStruct.PlayerLayer.value) return;

        isDamaged = false;
    }
    
    public void OnPlayerLayer()
    {
        OnStageLayer();
    }

    public override void OnEnemyLayer()
    {
        OnStageLayer();
    }
    
}

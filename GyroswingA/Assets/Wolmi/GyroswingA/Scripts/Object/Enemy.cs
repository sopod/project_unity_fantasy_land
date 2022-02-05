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


    [SerializeField] EnemyType enemyType;
    public int Type 
    { 
        get => (int) enemyType;
        set { enemyType = (EnemyType) value; }
    }


    StopWatch movementTimer;

    public bool movementsAllDone { get { return movementDatas.Count == 0; } }
    float knockDownTime;
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

    public void SetEnemy(GameObject stage, StageMovementValue stageVal, Options options)
    {
        bt = GetComponent<BT_Dragon>();

        creatureType = CreatureType.Enemy;
        movementDatas = new Queue<MovementData>();
        movementTimer = new StopWatch();

        this.moveSpeed = options.GetEnemyMoveSpeed(enemyType);
        this.rotSpeed = options.EnemyRotateSpeed;
        this.jumpPower = options.EnemyJumpPower;
        this.knockDownTime = options.EnemyKnockDownTime;

        checkEnemyToMove = false;
        isKnockDown = false;

        SetCreature(stage, stageVal, options);
    }

    public override void StartMoving()
    {
        base.StartMoving();
        
        bt.SetBT(options);
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
        GameCenter.Instance.OnMonsterKilled();
    }

    // -------------------------------------------------- damaged by player fire
    void OnTriggerEnter(Collider other)
    {
        if (IsPaused) return;

        int layer = (1 << other.gameObject.layer);

        if (layer != options.ShootProjectileLayer.value) return;

        GameObject p = options.ProjectilesSpawner.SpawnFireHitProjectile(this.gameObject);
        p.GetComponent<Projectile>().SetStart(options);
        OnDamagedAndMoveBack(true, other.transform.position, other.transform.forward, EnemyType.Max);
    }

    // -------------------------------------------------- damaged by player dash
    void OnCollisionStay(Collision collision)
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);
        if (layer != options.PlayerLayer.value) return;

        LivingCreature l = collision.gameObject.GetComponent<LivingCreature>();
        if (!l.IsAttacking || isDamaged) return;

        GameObject p = options.ProjectilesSpawner.SpawnDashHitProjectile(collision);
        p.GetComponent<Projectile>().SetStart(options);
        OnDamagedAndMoveBack(false, l.CenterPosition, l.CenterForward, EnemyType.Max);
    }

    // -------------------------------------------------- layer collision
    void OnCollisionEnter(Collision collision)
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);

        if (layer == options.StageLayer.value)
        {
            OnStageLayer();
        }
        else if (layer == options.FailZoneLayer.value)
        {
            OnFailZoneLayer();
        }
        else if (layer == options.EnemyLayer.value)
        {
            OnEnemyLayer();
        }
        else if (layer == options.PlayerLayer.value)
        {
            OnPlayerLayer(collision.gameObject.GetComponent<Player>());
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
        if (layer != options.PlayerLayer.value) return;

        isDamaged = false;
    }
    
    public void OnPlayerLayer(Player player)
    {
        OnStageLayer();
    }


    public override void OnEnemyLayer()
    {
        OnStageLayer();
    }
    
}

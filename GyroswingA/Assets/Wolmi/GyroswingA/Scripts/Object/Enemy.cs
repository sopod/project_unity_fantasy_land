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
    const float knockDownTime = 1.0f;

    void Update()
    {
        if (IsPaused) return;
        if (IsStopped) return;
        
        DoMovement();
        CheckMovementIsFinished();
        CheckKnockDownIsFinished();
        SetIsMovingAnimation();

        if (movementsAllDone && !isKnockDown)
        {
            bt.UpdateBT();
        }
    }

    void FixedUpdate()
    {
        if (IsPaused) return;

        AffectedByPhysics();
    }

    public void InitEnemy(GameObject stage, StageMovementValue stageVal, StageChanger stageChanger, Layers layer, ProjectileSpawner pjSpanwer)
    {
        values = SceneController.Instance.loaderGoogleSheet.ObjectDatas;

        bt = GetComponent<BT_Dragon>();

        creatureType = CreatureType.Enemy;
        movementDatas = new Queue<MovementData>();
        movementTimer = new StopWatch();

        curMoveSpeed = values.GetEnemyMoveSpeed(enemyType, values.MoveSpeed);

        checkEnemyToMove = false;
        isKnockDown = false;

        Init(stage, stageVal, layer, pjSpanwer);
    }

    public override void StartMoving()
    {
        base.StartMoving();
        
        bt.SetBT(layers, values);
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

        //bt.ResetBT();

        //if (movementDatas.Peek().btNode != null)
        //    movementDatas.Peek().btNode.SetFinishedFlag(true);

        movementDatas.Clear();

        checkEnemyToMove = false;

        InitAnimation();
        MakeEnemyDoIdleAnimation();
    }

    void KnockDown()
    {
        isKnockDown = true;
        DeleteAllMovementsAndStop();
        movementTimer.StartTimer(knockDownTime);
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

        if (layer != layers.ShootProjectileLayer.value) return;

        GameObject p = pjSpanwer.SpawnFireHitProjectile(this.gameObject);
        p.GetComponent<Projectile>().SetStart(pjSpanwer);
        OnDamagedAndMoveBack(true, other.transform.position, other.transform.forward, EnemyType.Max);

        //KnockDown();
    }

    // -------------------------------------------------- damaged by player dash
    void OnCollisionStay(Collision collision)
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);
        if (layer != layers.PlayerLayer.value) return;

        LivingCreature l = collision.gameObject.GetComponent<LivingCreature>();
        if (!l.IsAttacking || isDamaged) return;

        GameObject p = pjSpanwer.SpawnDashHitProjectile(collision);
        p.GetComponent<Projectile>().SetStart(pjSpanwer);
        OnDamagedAndMoveBack(false, l.CenterPosition, l.CenterForward, EnemyType.Max);

        //KnockDown();
    }

    // -------------------------------------------------- layer collision
    void OnCollisionEnter(Collision collision)
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);

        if (layer == layers.StageLayer.value)
        {
            OnStageLayer();
        }
        else if (layer == layers.FailZoneLayer.value)
        {
            OnFailZoneLayer();
        }
        else if (layer == layers.EnemyLayer.value)
        {
            OnEnemyLayer();
        }
        else if (layer == layers.PlayerLayer.value)
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
        if (layer != layers.PlayerLayer.value) return;

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

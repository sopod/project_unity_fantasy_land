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
    public event BackToPoolDelegate BackToPool;
    public void InvokeBackToPool() { BackToPool?.Invoke(); }
    [SerializeField] EnemyType enemyType = EnemyType.Max;
    public int Type
    {
        get => (int)enemyType;
        set
        {
            if (value >= (int)EnemyType.Max) return;
            enemyType = (EnemyType)value;
        }
    }

    BT_Dragon bt;
    Queue<MovementData> movementDatas;
    public bool movementsAllDone { get { return movementDatas.Count == 0; } }
    StopWatch movementTimer;



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

        aniPlay.DoMovingAnimation();

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

        if (movementDatas.Peek().movement == EnemyMovement.JumpForward) Jump();
    }

    void DoMovement()
    {
        if (!checkEnemyToMove || movementsAllDone || state.IsDead || state.IsAttacking) return;

        switch (movementDatas.Peek().movement)
        {
            case EnemyMovement.Wait: DoIdle(); break;
            case EnemyMovement.MoveForward: Move(1.0f); break;
            case EnemyMovement.MoveBackward: Move(-1.0f); break;
            case EnemyMovement.TurnRight: Turn(1.0f); break;
            case EnemyMovement.TurnLeft: Turn(-1.0f); break;
            case EnemyMovement.JumpForward: if (state.IsJumping) Move(1.0f); break;
            default: break;
        }
    }

    void DoIdle()
    {
        state.SetIdle();
        aniPlay.DoIdleAnimation();
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

        //InitAnimation();
        //MakeEnemyDoIdleAnimation();
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
    
    protected override void NotifyDead()
    {
        //soundPlayer.PlaySound(CreatureEffectSoundType.Dead, true);
        GameCenter.Instance.OnMonsterKilled();
    }

    protected override void OnDamagedByDash(Collision collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();
        if (!p.IsAttacking || isDamaged) return;

        if (IsHitBack(p.CenterPosition, p.CenterForward)) return;

        Vector3 dir = (transform.position - collision.transform.position).normalized;
        TakeDamage(dir, values.DashPowerToDamaged);
        pjSpanwer.SpawnDashHitProjectile(collision);
    }

    void OnDamagedByFire(Vector3 attackedPos)
    {
        Vector3 dir = (transform.position - attackedPos).normalized;
        TakeDamage(dir, values.FireBallPowerToDamaged);
        pjSpanwer.SpawnFireHitProjectile(this.gameObject);
    }

    // -------------------------------------------------- damaged by player fire
    void OnTriggerEnter(Collider other)
    {
        if (IsPaused) return;

        int layer = (1 << other.gameObject.layer);
        if (layer != layers.ShootProjectileLayer.value) return;

        OnDamagedByFire(other.transform.position);
        //KnockDown();
    }

    // -------------------------------------------------- damaged by player dash
    void OnCollisionStay(Collision collision)
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);
        if (layer != layers.PlayerLayer.value) return;

        OnDamagedByDash(collision);
        //KnockDown();
    }

    // -------------------------------------------------- layer collision
    void OnCollisionEnter(Collision collision)
    {
        if (IsPaused) return;
        int layer = (1 << collision.gameObject.layer);

        if (layer == layers.StageLayer.value || layer == layers.PlayerLayer.value || layer == layers.EnemyLayer.value)
        {
            OnStageLayer();
        }
        else if (layer == layers.FailZoneLayer.value)
        {
            Debug.Log("failZoneLayer");
            OnFailZoneLayer();
        }
        else
        {
            //OnNothingLayer();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);
        if (layer != layers.PlayerLayer.value) return;

        isDamaged = false;
    }
        
}

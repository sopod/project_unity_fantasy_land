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

public struct MovementData
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

    MonsterBT bt;
    StopWatch movementTimer;
    Queue<MovementData> movementDatas;
    public bool movementsAllDone { get { return movementDatas.Count == 0; } }

    //bool checkEnemyToMove;

    void Update()
    {
        if (IsPaused || IsStopped) return;

        DoMovement();
        CheckMovementIsFinished();

        aniPlay.DoMovingAnimation();

        if (movementsAllDone) bt.UpdateBT();
    }

    void FixedUpdate()
    {
        if (IsPaused) return;

        AffectedByPhysics();
    }

    public void InitEnemy(GameObject stage, StageMovementValue stageVal, StageChanger stageChanger, Layers layer, ProjectileSpawner pjSpanwer, Transform player)
    {
        Init(stage, stageVal, layer, pjSpanwer);

        bt = GetComponent<MonsterBT>();
        bt.SetBT(layers, values, player);

        creatureType = CreatureType.Enemy;
        movementDatas = new Queue<MovementData>();
        movementTimer = new StopWatch();

        curMoveSpeed = values.GetEnemyMoveSpeed(enemyType, values.MoveSpeed);

        //checkEnemyToMove = false;
    }

    public override void StartMoving()
    {
        base.StartMoving();
        bt.StartMoving();
    }

    public void AttackPlayer()
    {
        Dash();
    }

    public void AddMovement(MovementData input)
    {
        movementDatas.Enqueue(input);

        if (movementTimer.IsRunning) return;

        StartMovement();
    }

    void StartMovement()
    {
        //checkEnemyToMove = true;
        movementTimer.StartTimer(movementDatas.Peek().time);

        if (movementDatas.Peek().movement == EnemyMovement.JumpForward) Jump();
    }

    void DoMovement()
    {
        if (state.IsDamaged || state.IsDead || state.IsAttacking || movementsAllDone) return; //!checkEnemyToMove ||

        switch (movementDatas.Peek().movement)
        {
            case EnemyMovement.Wait:         DoIdle(); break;
            case EnemyMovement.MoveForward:  Move(1.0f); break;
            case EnemyMovement.MoveBackward: Move(-1.0f); break;
            case EnemyMovement.TurnRight:    Turn(1.0f); break;
            case EnemyMovement.TurnLeft:     Turn(-1.0f); break;
            case EnemyMovement.JumpForward:  if (state.IsJumping) Move(1.0f); break;
            default: break;
        }
    }

    void DoIdle()
    {
        if (state.IsIdle) return;

        state.SetIdle();
        aniPlay.DoIdleAnimation();
    }

    void CheckMovementIsFinished()
    {
        if (!movementTimer.IsFinished) return; //!checkEnemyToMove ||

        if (movementDatas.Peek().btNode != null)
            movementDatas.Peek().btNode.SetFinishedFlag(true);

        movementDatas.Dequeue();
        movementTimer.FinishTimer();
        //checkEnemyToMove = false

        if (!movementsAllDone) StartMovement();
    }

    protected override void OnDamagedByDash(Collision collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();

        if (p == null || !p.IsAttacking || state.IsDamaged) return;
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

    void OnTriggerEnter(Collider other)
    {
        if (IsPaused) return;

        int layer = (1 << other.gameObject.layer);

        if (layer == layers.ShootProjectileLayer.value)
        { 
            OnDamagedByFire(other.transform.position);
        }
        else if (layer == layers.StageBoundaryLayer.value)
        {
            state.IsInStageBoundary = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPaused) return;

        int layer = (1 << other.gameObject.layer);

        if (layer == layers.StageBoundaryLayer.value)
        {
            state.IsInStageBoundary = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);
        if (layer != layers.PlayerLayer.value) return;

        OnDamagedByDash(collision);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (IsPaused) return;
        int layer = (1 << collision.gameObject.layer);

        if (layer == layers.StageLayer.value || layer == layers.PlayerLayer.value || layer == layers.EnemyLayer.value)
        {
            OnStageLayer();
        }
        else if (layer == layers.StageBoundaryLayer.value)
        {
            state.IsInStageBoundary = true;
        }
        else if (layer == layers.FailZoneLayer.value)
        {
            OnFailZoneLayer();
        }
    }

    //void OnCollisionExit(Collision collision)
    //{
    //    if (IsPaused) return;

    //    int layer = (1 << collision.gameObject.layer);

    //    if (layer == layers.PlayerLayer.value)
    //    {
    //        //isDamaged = false;
    //    }
    //}
        
}

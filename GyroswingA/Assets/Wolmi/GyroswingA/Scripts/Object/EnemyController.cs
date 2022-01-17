using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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


public class EnemyController : LivingCreature, ISpawnableObject
{
    [SerializeField] EnemyType enemyType;
    public int Type 
    { 
        get { return (int) enemyType; }
        set { enemyType = (EnemyType) value; }
    }

    BT_Dragon bt;
    
    Queue<MovementData> movementDatas;

    TimeController movementTimer;

    public bool movementsAllDone { get { return movementDatas.Count == 0; } }
    float knockDownTime;
    bool checkEnemyToMove;
    bool isKnockDown;
    public bool IsEnemySet = false;


    void Update()
    {
        if (!IsPaused())
        {
            if (!IsStopped())
            {
                DoMovement();
                CheckMovementIsFinished();
                CheckKnockDownIsFinished();
                SetIsMovingAnimation();

                if (movementsAllDone && !isKnockDown)
                    bt.UpdateBT();

            }
        }
    }

    void FixedUpdate()
    {
        if (!IsPaused())
        {
            AffectedByPhysics();
        }
    }

    public void SetEnemy(GameObject stage, StageMovementValue stageVal, Options options)
    {
        bt = GetComponent<BT_Dragon>();

        creatureType = CreatureType.Enemy;
        movementDatas = new Queue<MovementData>();
        movementTimer = new TimeController();

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

        if (!movementTimer.IsRunning)
        {
            StartMovement();
        }
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
        if (checkEnemyToMove && !movementsAllDone)
        {
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
    }

    void CheckMovementIsFinished()
    {
        if (checkEnemyToMove && movementTimer.IsFinished)
        {
            if (movementDatas.Peek().btNode != null)
                movementDatas.Peek().btNode.SetFinishedFlag(true);

            movementDatas.Dequeue();
            movementTimer.FinishTimer();
            checkEnemyToMove = false;

            if (!movementsAllDone)
                StartMovement();
        }
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
        if (isKnockDown && movementTimer.IsFinished)
        {
            movementTimer.FinishTimer();
            isKnockDown = false;
        }
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

    void OnCollisionStay(Collision collision)
    {
        if (IsPaused()) return;

        int layer = (1 << collision.gameObject.layer);

        if (layer == options.PlayerLayer.value)
        {
            CheckDamagedToMoveBack(collision.gameObject.GetComponent<LivingCreature>());
        }
        else
        {
            //isDamaged = false;
        }

        if (layer == options.StageLayer.value)
        {
            OnStageLayer();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (IsPaused()) return;

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
            OnPlayerLayer(collision.gameObject.GetComponent<PlayerController>());
        }
        else
        {
            OnNothingLayer();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (IsPaused()) return;

        int layer = (1 << collision.gameObject.layer);

        if (layer == options.PlayerLayer.value)
        {
            isDamaged = false;
        }
    }

    protected override void NotifyDead()
    {
        GameManager.Instance.OnMonsterKilled();
    }

    public void OnPlayerLayer(PlayerController player) // when enemy first met player
    {

        // maybe don't need this when you make bt perfect....?



        //DeleteAllMovementsAndStop();

        //isKnockDown = true;
        //movementTimer.StartTimer(knockDownTime);

        //CheckDamagedToMoveBack(player);
    }


    public override void OnEnemyLayer()
    {
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position, 0.5f);
    }
}

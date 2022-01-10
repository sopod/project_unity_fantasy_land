using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyMovement
{
    None,
    MoveForward,
    MoveBackward,
    TurnRight,
    TurnLeft,
    JumpForward,
    Max
}



public class EnemyController : LivingCreature
{
    //NavMeshAgent agent;
    GameObject player;
    BT_Dragon bt;
    
    Queue<EnemyMovement> movementsToDo;
    Queue<Node> btNodes;
    TimeController movementTimer;
    public bool movementsAllDone { get { return movementsToDo.Count == 0; } }

    void Update()
    {
        if (!IsPaused())
        {
            if (!IsStopped())
            {
                DoMovement();
                CheckMovementIsFinished();

                if (movementsAllDone)
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

    public void SetEnemy(GameObject stage, GameObject player, float machineRadius, Options options)
    {
        this.player = player;
        bt = GetComponent<BT_Dragon>();

        movementsToDo = new Queue<EnemyMovement>();
        btNodes = new Queue<Node>();
        movementTimer = new TimeController();

        this.moveSpeed = options.PlayerMoveSpeed;
        this.rotSpeed = options.PlayerRotateSpeed;
        this.jumpPower = options.PlayerJumpPower;

        SetCreature(stage, machineRadius, options);
    }

    public override void StartMoving()
    {
        base.StartMoving();
        bt.SetBT();
    }
    
    public void AddEnemyMovement(EnemyMovement moveLikeThis, Node node)
    {
        movementsToDo.Enqueue(moveLikeThis);
        btNodes.Enqueue(node);

        if (!movementTimer.IsRunning)
        {
            StartMovement();
        }
    }

    float GetMovementTime(EnemyMovement movement)
    {
        if (movement == EnemyMovement.TurnLeft || movement == EnemyMovement.TurnRight)
            return 0.5f;
        return 1.0f;

    }

    void StartMovement()
    {
        EnemyMovement e = movementsToDo.Peek();

        movementTimer.StartTimer(GetMovementTime(e));

        if (e == EnemyMovement.JumpForward)
            Jump();
    }

    void DoMovement()
    {
        if (!movementsAllDone)
        {
            switch (movementsToDo.Peek())
            {
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
        if (movementTimer.IsFinished)
        {
            movementsToDo.Dequeue();
            btNodes.Peek().SetFinishedFlag(true);
            btNodes.Dequeue();
            movementTimer.FinishTimer();

            if (!movementsAllDone)
                StartMovement();
        }
    }

    public void AttackPlayer()
    {
        Dash();
    }


    //Vector3 GetDestinationToPlayer()
    //{
    //    Debug.DrawRay(GameManager.Instance.GetCenterPosOfPlayer, -stage.transform.up * rayDistanceToDetectPlayer, Color.blue);

    //    if (Physics.Raycast(GameManager.Instance.GetCenterPosOfPlayer, -stage.transform.up, out hit, rayDistanceToDetectPlayer,
    //            stageLayer))
    //    {
    //        return hit.point;
    //    }

    //    Debug.Log("couldn't find the target player");
    //    return this.transform.position;
    //}


    void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer) == stageLayer.value)
        {
            isJumping = false;
            isOnStage = true;
            isFlying = false;
            isOnPlatform = false;
        }
        else if ((1 << collision.gameObject.layer) == failZoneLayer.value)
        {
            Debug.Log("Enemy died");

            isJumping = false;
            isOnStage = false;
            isFlying = false;
            isOnPlatform = false;
        }
        else if ((1 << collision.gameObject.layer) == platformLayer.value)
        {
            isJumping = false;
            isOnStage = true;
            isFlying = false;
            isOnPlatform = true;
        }
        else if ((1 << collision.gameObject.layer) == playerLayer.value)
        {

        }
        else if ((1 << collision.gameObject.layer) == enemyLayer.value)
        {

        }
        else
        {
            isOnStage = false;
            isFlying = true;
            isOnPlatform = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position, 0.5f);
    }
}

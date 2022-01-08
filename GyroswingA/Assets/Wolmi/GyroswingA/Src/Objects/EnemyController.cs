using System.Collections;
using System.Collections.Generic;
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



public class EnemyController : LivingCreatures
{
    //NavMeshAgent agent;
    GameObject player;
    BT_Dragon bt;

    float movementTimeMax { get { return 1.0f; } } // move for 1 seconds
    EnemyMovement movementCur;
    TimeController movementTimer;

    void Update()
    {
        if (!IsPaused())
        {
            if (!IsStopped())
            {
                DoMovement();

                if (movementCur == EnemyMovement.None)
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
        movementCur = EnemyMovement.None;
        movementTimer = new TimeController();

        SetCreature(stage, machineRadius, options);
    }

    public override void StartMoving()
    {
        base.StartMoving();
        bt.SetBT();
    }
    
    public void StartMoveEnemy(EnemyMovement moveLikeThis)
    {
        if (movementCur == EnemyMovement.None)
        {
            movementCur = moveLikeThis;
            movementTimer.StartTimer(movementTimeMax);

            if (moveLikeThis == EnemyMovement.JumpForward)
                Jump();
        }
    }

    void FinishMoveEnemy()
    {
        movementCur = EnemyMovement.None;
        movementTimer.FinishTimer();
    }

    void DoMovement()
    {
        if (movementCur != EnemyMovement.None)
        {
            if (!movementTimer.IsFinished)
            {
                switch (movementCur)
                {
                    case EnemyMovement.MoveForward: Move(1.0f); break;
                    case EnemyMovement.MoveBackward: Move(-1.0f); break;
                    case EnemyMovement.TurnRight: Turn(1.0f); break;
                    case EnemyMovement.TurnLeft: Turn(-1.0f); break;
                    case EnemyMovement.JumpForward: if (isJumping) Move(1.0f); break;
                }
            }
            else
            {
                FinishMoveEnemy();
            }
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
        //Gizmos.DrawSphere(transform.position, 0.8f);
    }
}

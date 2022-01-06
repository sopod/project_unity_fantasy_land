using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : LivingCreatures
{
    NavMeshAgent agent;
    GameObject player;
    [HideInInspector] public Vector3 orgPos;
    RaycastHit hit;

    protected StageMovementValue values;
    protected bool moveAlongStage;


    float rayDistance
    {
        get { return 1.0f; }
    }


    void Update()
    {
        if (!IsPaused())
        {
            if (!IsStopped())
            {
                //agent.SetDestination(GetDestinationToPlayer());

                //AffectedBySpin();

                if (moveAlongStage)
                    MoveAlongStage();
            }
        }
    }

    void FixedUpdate()
    {
        if (!IsPaused())
        {
            AffectedByGravity();
            //FreezeLocalVelocity();
        }
    }


    public void SetEnemy(GameObject stage, GameObject player, float moveSpeed, float rotSpeed, float jumpPower,
        float machineRadius, float spinSpeed, bool isSpiningCW)
    {
        moveAlongStage = false;
        this.player = player;

        //agent = GetComponent<NavMeshAgent>();
        //agent.speed = moveSpeed;
        //agent.angularSpeed = rotSpeed;
        //agent.acceleration = 0.1f;
        //agent.radius = 1.0f;

        SetCreature(stage, moveSpeed, rotSpeed, jumpPower, machineRadius, spinSpeed, isSpiningCW);
    }

    Vector3 GetDestinationToPlayer()
    {
        Debug.DrawRay(GameManager.Instance.GetCenterPosOfPlayer, -stage.transform.up * rayDistance, Color.blue);

        if (Physics.Raycast(GameManager.Instance.GetCenterPosOfPlayer, -stage.transform.up, out hit, rayDistance,
                stageLayer))
        {
            return hit.point;
        }

        Debug.Log("couldn't find the target player");
        return this.transform.position;
    }

    void AffectedByGravity()
    {
        rb.velocity -= stage.transform.up * gravity * Time.fixedDeltaTime;
    }

    public void SetValuesThenMove(StageMovementValue values)
    {
        this.values = values;
        moveAlongStage = true;
    }

    void MoveAlongStage()
    {
        if (isOnStage && !IsPaused() && !GameManager.Instance.IsMachineStopped)
        {
            Vector3 centerforTurn = (values.isSwingRight) ? Vector3.left : -Vector3.left;
            Vector3 centerforSpin = (values.isSpiningCW) ? values.stageUpDir : -values.stageUpDir;
            Vector3 resPos = transform.position;

            Quaternion turnQuat = new Quaternion();
            Quaternion spinQuat = new Quaternion();

            if (values.isSwinging)
                resPos += values.swingPosCur;

            if (values.isTurning)
            {
                turnQuat = Quaternion.AngleAxis(values.swingAngleCur, centerforTurn);
                resPos = (turnQuat * (resPos - stage.transform.position) + stage.transform.position);
            }

            if (values.isSpining)
            {
                spinQuat = Quaternion.AngleAxis(values.spinAngleCur, centerforSpin);
                resPos = (spinQuat * (resPos - stage.transform.position) + stage.transform.position);
            }

            // apply
            if (values.isSpining)
                transform.rotation = spinQuat * transform.rotation;
            if (values.isTurning)
                transform.rotation = turnQuat * transform.rotation;

            transform.position = resPos;

            moveAlongStage = false;
        }
    }

    void AffectedBySpin()
    {
        if (isOnStage && !GameManager.Instance.IsMachineStopped)
        {
            Vector3 dir = GetDirectionFromStageToCreature();

            agent.velocity += dir * machineRadius * _spinSpeedUp * Mathf.Deg2Rad * Time.fixedDeltaTime; // weird...
        }
    }

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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingCreatures : MovingThings
{
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected LayerMask stageLayer;
    [SerializeField] protected LayerMask failZoneLayer;
    [SerializeField] protected LayerMask platformLayer;
    [SerializeField] protected GameObject stage;
    [SerializeField] protected GameObject centerOfCreature;

    protected Rigidbody rb;

    protected float moveSpeed;
    protected float rotSpeed;
    protected float jumpPower;

    protected float gravity { get { return 9.8f; } }

    [SerializeField] protected bool isFlying;
    [SerializeField] protected bool isJumping;
    [SerializeField] protected bool isOnStage;
    [SerializeField] protected bool isOnPlatform;

    protected float machineRadius;
    protected float machineSpinSpeed;
    protected bool isMachineSpiningCW;

    protected float _spinSpeedUp;

    void FixedUpdate()
    {
        if (!IsPaused())
        {
            AffectedByGravity();
            AffectedBySpin();
            
            //FreezeLocalVelocity();
        }
    }

    public void SetCreature(GameObject stage, float moveSpeed, float rotSpeed, float jumpPower, 
        float machineRadius, float spinSpeed, bool isSpiningCW)
    {
        this.stage = stage;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 1;
        rb.drag = 5;
        rb.angularDrag = 10;
        
        this.moveSpeed = moveSpeed;
        this.rotSpeed = rotSpeed;
        this.jumpPower = jumpPower;

        this.machineRadius = machineRadius;
        this.machineSpinSpeed = 0.0f;
        _spinSpeedUp = 0.0f;

        InitCreature(spinSpeed, isSpiningCW);
    }

    public void InitCreature(float spinSpeed, bool isSpiningCW)
    {
        ChangeMachineValue(spinSpeed);
        ChangeSpinDirection(isSpiningCW);

        isFlying = false;
        isJumping = false;
        isOnStage = true;
        isOnPlatform = false;

        InitMovingThings();
    }

    public void ChangeMachineValue(float spinSpeed)
    {
        if (machineSpinSpeed < spinSpeed)
            _spinSpeedUp += 0.1f;
        else
            _spinSpeedUp -= 0.1f;

        if (_spinSpeedUp > 5.0f)
            _spinSpeedUp = 5.0f;
        else if (_spinSpeedUp < 0.0f)
            _spinSpeedUp = 0.0f;

        machineSpinSpeed = spinSpeed;
    }

    public void ChangeSpinDirection(bool isSpiningCW)
    {
        isMachineSpiningCW = isSpiningCW;
    }

    protected void AffectedByGravity()
    {
        rb.velocity -= stage.transform.up * gravity * Time.fixedDeltaTime;

        //if (isOnStage && !GameManager.Instance.IsMachineStopped)
        //{
        //    rb.velocity -= stage.transform.up * gravity * Time.fixedDeltaTime;
        //}
        //else
        //{
        //    rb.velocity -= Vector3.up * gravity * Time.fixedDeltaTime;
        //}
    }

    public void MoveAlongStage(Vector3 swingPosCur, bool isSwingRight, float swingAngleCur, bool isSpiningCW, float spinAngleCur, Vector3 stageUpDir,
        bool isSwinging, bool isTurning, bool isSpining)
    {
        if (isOnStage && !GameManager.Instance.IsMachineStopped)
        {
            Vector3 centerforTurn = (isSwingRight) ? Vector3.left : -Vector3.left;
            Vector3 centerforSpin = (isSpiningCW) ? stageUpDir : -stageUpDir;
            Vector3 resPos = rb.position;

            Quaternion turnQuat = new Quaternion();
            Quaternion spinQuat = new Quaternion();

            if (isSwinging)
                resPos += swingPosCur;

            if (isTurning)
            {
                turnQuat = Quaternion.AngleAxis(swingAngleCur, centerforTurn);
                resPos = (turnQuat * (resPos - stage.transform.position) + stage.transform.position); //transform.RotateAround(stage.transform.position, Vector3.up, machineSpinSpeed * Time.fixedDeltaTime);
            }

            if (isSpining)
            {
                spinQuat = Quaternion.AngleAxis(spinAngleCur, centerforSpin);
                resPos = (spinQuat * (resPos - stage.transform.position) + stage.transform.position);
            }

            // apply
            rb.rotation = spinQuat * rb.rotation;
            rb.rotation = turnQuat * rb.rotation;
            rb.position = resPos;
        }
    }

    protected Vector3 GetDirectionFromStageToPlayer()
    {
        Vector3 centerPosOfPlayer = centerOfCreature.transform.position; // GetComponent<Renderer>().bounds.center;
        Vector3 fromStageToPlayer = centerPosOfPlayer - stage.transform.position;
        float height = Vector3.Dot(fromStageToPlayer, stage.transform.up.normalized);

        Vector3 parallelPos = stage.transform.position + new Vector3(0, height, 0);
        Vector3 res = (centerPosOfPlayer - parallelPos).normalized;

        Debug.DrawRay(centerPosOfPlayer, res, Color.red);

        return res;
    }

    protected void AffectedBySpin()
    {
        if (isOnStage && !GameManager.Instance.IsMachineStopped)
        {
            Vector3 dir = GetDirectionFromStageToPlayer();

            rb.velocity += dir * machineRadius * (55.5f + _spinSpeedUp) * Mathf.Deg2Rad * Time.fixedDeltaTime;

            //if (GameManager.Instance.IsRightSpin)
            //    rb.velocity += stage.transform.forward * machineRadius * (62.0f + _spinSpeedUp) * Mathf.Deg2Rad * Time.fixedDeltaTime;
            //else
            //    rb.velocity += -stage.transform.forward * machineRadius * (62.0f + _spinSpeedUp) * Mathf.Deg2Rad * Time.fixedDeltaTime;
            //Debug.DrawRay(transform.position, stage.transform.forward, Color.red);
        }
    }

    protected void FreezeLocalVelocity()
    {
        if (isOnPlatform)
        {
            //rb.angularVelocity = new Vector3(0, rb.angularVelocity.y, 0);

            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
            localVelocity.x = 0;
            localVelocity.z = 0;
            rb.velocity = transform.TransformDirection(localVelocity);


            //Vector3 myUp = transform.up;
            //Vector3 machineUp = stage.transform.up;

            //Vector3 slerpDir = Vector3.Slerp(myUp, machineUp, Time.fixedDeltaTime * 10.0f);
            //rb.rotation *= Quaternion.FromToRotation(myUp, slerpDir);


            //Vector3 distance = stage.transform.position - transform.position;
            //Vector3 idealPosition = stage.transform.position - transform.forward * distance.magnitude;
            //Vector3 correction = idealPosition - transform.position;

            //correction = transform.InverseTransformDirection(correction);
            //correction.y = 0;
            //correction = transform.TransformDirection(correction);

            //rb.velocity += correction * 2.0f;
        }
    }

}

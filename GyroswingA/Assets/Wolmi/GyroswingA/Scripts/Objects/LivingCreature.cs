using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingCreature : MovingThing
{
    protected Options options;
    protected StageMovementValue stageVal;

    protected GameObject stage;
    [SerializeField] protected GameObject centerOfCreature;

    protected Rigidbody rb;
    protected Animator ani;
    
    [SerializeField] protected bool isMoving;
    [SerializeField] protected bool isTurning;
    [SerializeField] protected bool isJumping;
    [SerializeField] protected bool isDoingSkill;
    [SerializeField] protected bool isOnStage;
    [SerializeField] protected bool isDead;
    [SerializeField] protected bool isDamaged;
    [SerializeField] protected bool isAffectedByStage;

    protected float moveSpeed;
    protected float rotSpeed;
    protected float jumpPower;
    
    protected float _spinSpeedUp;

    public Vector3 CenterPosition { get { return centerOfCreature.transform.position; } }
    public bool IsDoingSkill { get { return isDoingSkill; } }

    protected void SetCreature(GameObject stage, StageMovementValue stageVal, Options options)
    {
        this.options = options;
        this.stageVal = stageVal;

        this.stage = stage;
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.mass = 1;
        rb.drag = 5;
        rb.angularDrag = 10;
        
        _spinSpeedUp = 0.0f;
    }

    public void ResetCreature(LevelValues values)
    {
        isMoving = false;
        isTurning = false;
        isJumping = false;
        isDoingSkill = false;
        isOnStage = true;
        isDead = false;
        isAffectedByStage = true;
        isDamaged = false;

        PauseMoving();
    }
    
    protected void AffectedByPhysics()
    {
        AffectedByGravity();
        //AffectedBySpin();
        FreezeLocalXZRotation();
    }

    public void InitAnimation()
    {
        ani.SetFloat("MoveFront", 0.0f);
        ani.SetFloat("TurnRight", 0.0f);
        ani.SetBool("IsMoving", false);
        ani.SetBool("IsJumping", false);
        ani.SetBool("IsDead", false);
    }

    protected void SetIsMovingAnimation()
    {
        ani.SetBool("IsMoving", isMoving || isTurning);
    }

    public void Move(float key)
    {
        if (isDoingSkill) return;

        if ((Mathf.Abs(key) > 0.1f))
            isMoving = true;
        else
            isMoving = false;

        ani.SetFloat("MoveFront", key);

        rb.position += transform.forward * key * moveSpeed * Time.deltaTime;
    }

    public void Turn(float key)
    {
        if (isDoingSkill) return;

        if ((Mathf.Abs(key) > 0.1f))
            isTurning = true;
        else
            isTurning = false;

        ani.SetFloat("TurnRight", key);

        float angle = key * rotSpeed * Time.fixedDeltaTime;
        rb.rotation *= Quaternion.AngleAxis(angle, Vector3.up);
    }

    public void Jump()
    {
        if (isJumping || isDoingSkill) return;

        ani.SetBool("IsJumping", true);

        isJumping = true;
        isOnStage = false;
        
        rb.AddForce(stage.transform.up * jumpPower, ForceMode.Impulse);

    }

    public void Dash()
    {
        if (isJumping || isDoingSkill) return;

        ani.SetTrigger("JustDashed");
        rb.AddForce(transform.forward * options.DashPowerToHit, ForceMode.Impulse);
        
        isDoingSkill = true;

        Invoke("SetCanDoAnything", options.WaitForAnotherDash);
    }

    public void Fire()
    {
        if (isJumping || isDoingSkill) return;
        
        ani.SetTrigger("JustFired");

        // do something





        isDoingSkill = true;

        Invoke("SetCanDoAnything", options.WaitForAnotherDash);
    }
    protected void CheckDamagedToMoveBack(LivingCreature otherCreature)
    {
        bool isAttacked = otherCreature.IsDoingSkill;

        if (isAttacked && !isDamaged)
        {
            Vector3 dir = (CenterPosition - otherCreature.CenterPosition).normalized;

            rb.AddForce(dir * options.DashPowerToDamaged, ForceMode.Impulse);

            isDamaged = true;
        }
    }

    public void SetCanDoAnything()
    {
        isDoingSkill = false;
    }

    protected void AffectedByGravity()
    {
        rb.velocity -= stage.transform.up * options.Gravity * Time.fixedDeltaTime;
    }

    protected void AffectedBySpin()
    {
        if (isOnStage && !GameManager.Instance.IsMachineStopped)
        {
            Vector3 dir = GetDirectionFromStageToCreature();

            rb.velocity += (dir * stageVal.Radius * (55.5f + _spinSpeedUp) * Mathf.Deg2Rad * Time.fixedDeltaTime);
        }
    }

    protected Vector3 GetDirectionFromStageToCreature()
    {
        Vector3 centerPosOfCreature = centerOfCreature.GetComponent<Renderer>().bounds.center;
        Vector3 fromStageToCreature = centerPosOfCreature - stage.transform.position;
        float height = Vector3.Dot(fromStageToCreature, stage.transform.up.normalized);

        Vector3 parallelPos = stage.transform.position + new Vector3(0, height, 0);
        Vector3 res = (centerPosOfCreature - parallelPos).normalized;

        Debug.DrawRay(centerPosOfCreature, res, Color.red);

        return res;
    }

    public void MoveAlongStage()
    {
        if (!IsPaused() && !GameManager.Instance.IsMachineStopped && isAffectedByStage)
        {
            Vector3 centerForSpin = (options.IsSpiningCW) ? stage.transform.up : -stage.transform.up;
            Vector3 resPos = rb.position;

            // swing
            if (options.IsMachineSwinging)
                resPos += stageVal.SwingPosCur;

            // spin
            if (options.IsMachineSpining && isOnStage && !isJumping)
            {
                Quaternion spinQuat = Quaternion.AngleAxis(stageVal.SpinAngleCur, centerForSpin);
                resPos = (spinQuat * (resPos - stage.transform.position) + stage.transform.position);
                rb.rotation = spinQuat * rb.rotation;
            }

            // apply
            rb.position = resPos;
        }
    }

    protected void FreezeLocalXZRotation()
    {
        if (isAffectedByStage)
        {
            Quaternion turnQuat = Quaternion.FromToRotation(centerOfCreature.transform.up, stage.transform.up);
            rb.rotation = turnQuat * rb.rotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPaused()) return;
            
        int layer = (1 << other.gameObject.layer);

        if (layer == options.StageBoundaryLayer.value)
        {
            isAffectedByStage = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (IsPaused()) return;

        int layer = (1 << other.gameObject.layer);

        if (layer == options.StageBoundaryLayer.value)
        {
            isAffectedByStage = false;
        }
    }

    public void OnStageLayer()
    {
        isJumping = false;
        isOnStage = true;

        ani.SetBool("IsJumping", false);
    }

    public void OnFailZoneLayer()
    {
        if (isDead) return;

        isJumping = false;
        isOnStage = false;
        isDead = true;

        NotifyDead();

        ani.SetBool("IsDead", true);
        ani.SetBool("IsJumping", false);
    }

    protected abstract void NotifyDead();

    public abstract void OnEnemyLayer();

    public void OnNothingLayer()
    {
        isOnStage = false;
    }
    


    //protected void FreezeLocalVelocity()
    //{
    //    if (isOnPlatform)
    //    {
    //        //rb.angularVelocity = new Vector3(0, rb.angularVelocity.y, 0);

    //        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
    //        localVelocity.x = 0;
    //        localVelocity.z = 0;
    //        rb.velocity = transform.TransformDirection(localVelocity);


    //        //Vector3 myUp = transform.up;
    //        //Vector3 machineUp = stage.transform.up;

    //        //Vector3 slerpDir = Vector3.Slerp(myUp, machineUp, Time.fixedDeltaTime * 10.0f);
    //        //rb.rotation *= Quaternion.FromToRotation(myUp, slerpDir);


    //        //Vector3 distance = stage.transform.position - transform.position;
    //        //Vector3 idealPosition = stage.transform.position - transform.forward * distance.magnitude;
    //        //Vector3 correction = idealPosition - transform.position;

    //        //correction = transform.InverseTransformDirection(correction);
    //        //correction.y = 0;
    //        //correction = transform.TransformDirection(correction);

    //        //rb.velocity += correction * 2.0f;
    //    }
    //}

    //void MoveAlongStage()
    //{
    //    if (isOnStage && !IsPaused() && !GameManager.Instance.IsMachineStopped)
    //    {
    //        Vector3 centerforTurn = (values.IsSwingRight) ? Vector3.left : -Vector3.left;
    //        Vector3 centerforSpin = (values.IsSpiningCW) ? values.StageUpDir : -values.StageUpDir;
    //        Vector3 resPos = transform.position;

    //        Quaternion turnQuat = new Quaternion();
    //        Quaternion spinQuat = new Quaternion();

    //        if (values.isSwinging)
    //            resPos += values.SwingPosCur;

    //        if (values.isTurning)
    //        {
    //            turnQuat = Quaternion.AngleAxis(values.SwingAngleCur, centerforTurn);
    //            resPos = (turnQuat * (resPos - stage.transform.position) + stage.transform.position);
    //        }

    //        if (values.isSpining)
    //        {
    //            spinQuat = Quaternion.AngleAxis(values.SpinAngleCur, centerforSpin);
    //            resPos = (spinQuat * (resPos - stage.transform.position) + stage.transform.position);
    //        }

    //         apply
    //        if (values.isSpining)
    //            transform.rotation = spinQuat * transform.rotation;
    //        if (values.isTurning)
    //            transform.rotation = turnQuat * transform.rotation;

    //        transform.position = resPos;

    //        moveAlongStage = false;
    //    }
    //}


}

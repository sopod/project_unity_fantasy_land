using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingCreature : MovingThing
{
    protected LayerMask playerLayer;
    protected LayerMask enemyLayer;
    protected LayerMask stageLayer;
    protected LayerMask failZoneLayer;
    protected LayerMask platformLayer;
    protected LayerMask stagePoleLayer;

    [SerializeField] protected GameObject centerOfCreature;
    protected GameObject stage;

    protected Rigidbody rb;
    protected Animator ani;

    [SerializeField] protected bool isFlying;
    [SerializeField] protected bool isJumping;
    [SerializeField] protected bool isDoingSkill;
    [SerializeField] protected bool isOnStage;
    [SerializeField] protected bool isOnPlatform;
    [SerializeField] protected bool isDead;

    protected float moveSpeed;
    protected float rotSpeed;
    protected float jumpPower;

    float gravity;
    float waitTimeAfterDash;
    float dashPower;

    protected float machineRadius;
    protected float machineSpinSpeed;
    protected bool isMachineSpiningCW;

    protected float _spinSpeedUp;

    public Vector3 CenterPosition { get { return centerOfCreature.transform.position; } }

    protected void SetCreature(GameObject stage, float machineRadius, Options options)
    {
        this.stage = stage;
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 1;
        rb.drag = 5;
        rb.angularDrag = 10;

        this.machineRadius = machineRadius;
        this.machineSpinSpeed = 0.0f;
        _spinSpeedUp = 0.0f;

        gravity = options.Gravity;
        waitTimeAfterDash = options.WaitTimeAfterDash;
        dashPower = options.PlayerDashPower;

        SetLayers();
        InitCreature(options.GetCurLevelValues());
    }

    public void InitCreature(LevelValues values)
    {
        ChangeMachineValues(values);

        isFlying = false;
        isJumping = false;
        isDoingSkill = false;
        isOnStage = true;
        isOnPlatform = false;
        isDead = false;

        PauseMoving();
    }

    public void SetLayers()
    {
        playerLayer = GameManager.Instance.playerLayer;
        enemyLayer = GameManager.Instance.enemyLayer;
        stageLayer = GameManager.Instance.stageLayer;
        failZoneLayer = GameManager.Instance.failZoneLayer;
        platformLayer = GameManager.Instance.platformLayer;
        stagePoleLayer = GameManager.Instance.stagePoleLayer;
    }

    public void ChangeMachineValues(LevelValues values)
    {
        if (machineSpinSpeed < values.MachineSpinSpeed)
            _spinSpeedUp += 0.1f;
        else
            _spinSpeedUp -= 0.1f;

        if (_spinSpeedUp > 5.0f)
            _spinSpeedUp = 5.0f;
        else if (_spinSpeedUp < 0.0f)
            _spinSpeedUp = 0.0f;

        machineSpinSpeed = values.MachineSpinSpeed;
    }

    protected void AffectedByPhysics()
    {
        AffectedByGravity();
        //AffectedBySpin();
        FreezeLocalXZRotation();
    }

    public void Move(float key)
    {
        if (isDoingSkill) return;

        ani.SetFloat("MoveFront", key);
        ani.SetBool("IsMoving", (key != 0.0f));

        rb.position += transform.forward * key * moveSpeed * Time.deltaTime;


        //if (!_isJumping)
        //{
        //rb.velocity += transform.forward * key.GetVerticalKey() * moveSpeed * 10.0f * Time.deltaTime;


        //transform.position += transform.forward * key.GetVerticalKey() * moveSpeed * Time.deltaTime;

        //Vector3 moveVec = transform.forward * key.GetVerticalKey();
        //moveVec.Normalize();
        //transform.position += moveVec * 2.0f * Time.fixedDeltaTime;

        //Vector3 moveVec = transform.forward * key.GetVerticalKey() * 2.0f;
        //moveVec.y = rb.velocity.y;
        //rb.velocity = moveVec;

        //Vector3 moveVec = transform.forward * key.GetVerticalKey() * 200.0f * Time.fixedDeltaTime;
        //moveVec.y = rb.velocity.y;
        //rb.velocity = moveVec;

        //Vector3 moveVec = transform.forward * key.GetVerticalKey();
        //moveVec.y = 0.0f;
        //moveVec.Normalize();
        //rb.MovePosition(rb.position + moveVec * 2.0f * Time.fixedDeltaTime);
        //}
    }

    public void Turn(float key)
    {
        if (isDoingSkill) return;

        ani.SetFloat("TurnRight", key);
        ani.SetBool("IsMoving", (key != 0.0f));

        float angle = key * rotSpeed * Time.fixedDeltaTime;
        rb.rotation *= Quaternion.AngleAxis(angle, Vector3.up);


        //    float angle = key.GetHorizontalKey() * rotSpeed * Time.fixedDeltaTime;
        //    rb.MoveRotation(transform.localRotation * Quaternion.AngleAxis(angle, Vector3.up));
        //transform.localRotation *= Quaternion.AngleAxis(angle, Vector3.up);
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
        rb.AddForce(transform.forward * dashPower);

        isDoingSkill = true;

        Invoke("SetCanDoAnything", waitTimeAfterDash);
    }

    public void Fire()
    {
        // do something



        ani.SetTrigger("JustFired");
    }

    public void SetCanDoAnything()
    {
        isDoingSkill = false;
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
    protected void AffectedBySpin()
    {
        if (isOnStage && !GameManager.Instance.IsMachineStopped)
        {
            Vector3 dir = GetDirectionFromStageToCreature();

            rb.velocity += (dir * machineRadius * (55.5f + _spinSpeedUp) * Mathf.Deg2Rad * Time.fixedDeltaTime);

            //if (GameManager.Instance.IsRightSpin)
            //    rb.velocity += stage.transform.forward * machineRadius * (62.0f + _spinSpeedUp) * Mathf.Deg2Rad * Time.fixedDeltaTime;
            //else
            //    rb.velocity += -stage.transform.forward * machineRadius * (62.0f + _spinSpeedUp) * Mathf.Deg2Rad * Time.fixedDeltaTime;
            //Debug.DrawRay(transform.position, stage.transform.forward, Color.red);
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

    public void MoveAlongStage(StageMovementValue values)
    {
        if (!IsPaused() && !GameManager.Instance.IsMachineStopped)
        {
            Vector3 centerforSpin = (values.isSpiningCW) ? values.stageUpDir : -values.stageUpDir;
            Vector3 resPos = rb.position;
            
            // swing
            if (values.isSwinging)
                resPos += values.swingPosCur;

            // spin
            if (values.isSpining && isOnStage)
            {
                Quaternion spinQuat = Quaternion.AngleAxis(values.spinAngleCur, centerforSpin);
                resPos = (spinQuat * (resPos - stage.transform.position) + stage.transform.position);
                rb.rotation = spinQuat * rb.rotation;
            }
            
            // apply
            rb.position = resPos;
        }
    }

    protected void FreezeLocalXZRotation()
    {
        Quaternion turnQuat = Quaternion.FromToRotation(centerOfCreature.transform.up, stage.transform.up);
        rb.rotation = turnQuat * rb.rotation;
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
    //        Vector3 centerforTurn = (values.isSwingRight) ? Vector3.left : -Vector3.left;
    //        Vector3 centerforSpin = (values.IsSpiningCW) ? values.stageUpDir : -values.stageUpDir;
    //        Vector3 resPos = transform.position;

    //        Quaternion turnQuat = new Quaternion();
    //        Quaternion spinQuat = new Quaternion();

    //        if (values.isSwinging)
    //            resPos += values.swingPosCur;

    //        if (values.isTurning)
    //        {
    //            turnQuat = Quaternion.AngleAxis(values.swingAngleCur, centerforTurn);
    //            resPos = (turnQuat * (resPos - stage.transform.position) + stage.transform.position);
    //        }

    //        if (values.isSpining)
    //        {
    //            spinQuat = Quaternion.AngleAxis(values.spinAngleCur, centerforSpin);
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

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
    [SerializeField] public GameObject centerOfCreature;
    protected GameObject stage;

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

    protected void SetCreature(GameObject stage, float moveSpeed, float rotSpeed, float jumpPower, 
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




}

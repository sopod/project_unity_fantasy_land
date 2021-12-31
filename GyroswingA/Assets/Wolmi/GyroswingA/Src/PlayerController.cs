using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MovingThings
{
    [SerializeField] LayerMask stageLayer;
    [SerializeField] LayerMask failZoneLayer;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] GameObject stage;
    [SerializeField] GameObject centerOfPlayer;

    KeyManager key;
    Rigidbody rb;

    float gravity = 9.8f;
    float moveSpeed;
    float rotSpeed;
    float jumpPower;

    bool isFlying;
    bool isJumping;
    bool isOnStage;
    bool isOnPlatform;

    float machineRadius;
    float machineSpinSpeed;
    bool isMachineSpiningCW;

    float _spinSpeedUp;


    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (!IsPaused())
        {
            if (!IsStopped())
            {
                MovePlayer();
                TurnPlayer();
                JumpPlayer();
            }

            AffectedByGravity();
            AffectedBySpin();
            //FreezeLocalVelocity();
        }
    }
    public void InitPlayer(KeyManager keyManager, float moveSpeed, float rotSpeed, float jumpPower,
                        float machineRadius, float spinSpeed, bool isSpiningCW)
    {
        key = keyManager;
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

    void MovePlayer()
    {        
        //if (!_isJumping)
        {
            rb.position += transform.forward * key.GetVerticalKey() * moveSpeed * Time.deltaTime;


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
        }
    }

    void TurnPlayer()
    {
    //    float angle = key.GetHorizontalKey() * rotSpeed * Time.fixedDeltaTime;
    //    rb.MoveRotation(transform.localRotation * Quaternion.AngleAxis(angle, Vector3.up));
        //transform.localRotation *= Quaternion.AngleAxis(angle, Vector3.up);
        
        float angle = key.GetHorizontalKey() * rotSpeed * Time.fixedDeltaTime;
        rb.rotation *= Quaternion.AngleAxis(angle, Vector3.up);
    }

    void JumpPlayer()
    {
        if (key.IsJumpKeyPressed() && !isJumping)
        {
            isJumping = true;
            isOnStage = false;

            //rb.velocity += transform.up * jumpPower;
            rb.AddForce(stage.transform.up * jumpPower, ForceMode.Impulse);
        }
    }

    void AffectedByGravity()
    {
        if (isOnStage && !GameManager.Instance.IsMachineStopped)
        {
            rb.velocity -= stage.transform.up * gravity * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity -= Vector3.up * gravity * Time.fixedDeltaTime;
        }
    }

    public void MovePlayerAlongStage(Vector3 swingPosCur, bool isSwingRight, float swingAngleCur, bool isSpiningCW, float spinAngleCur, Vector3 stageUpDir,
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

    Vector3 GetDirectionFromStageToPlayer()
    {
        Vector3 centerPosOfPlayer = centerOfPlayer.GetComponent<Renderer>().bounds.center;
        Vector3 fromStageToPlayer = centerPosOfPlayer - stage.transform.position;
        float height = Vector3.Dot(fromStageToPlayer, stage.transform.up.normalized);

        Vector3 parallelPos = stage.transform.position + new Vector3(0, height, 0);
        Vector3 res = (centerPosOfPlayer - parallelPos).normalized;
        
        Debug.DrawRay(centerPosOfPlayer, res, Color.red);

        return res;
    }

    void AffectedBySpin()
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

    void FreezeLocalVelocity()
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
            Debug.Log("You fell. Game Over.");

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
        else
        {
            isOnStage = false;
            isFlying = true;
            isOnPlatform = false;
        }
    }

}

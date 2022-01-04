using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : LivingCreatures
{
    KeyManager key;


    private void Update()
    {
        if (!IsPaused())
        {
            if (!IsStopped())
            {
                MovePlayer();
                TurnPlayer();
                JumpPlayer();

                Dash();
            }
        }
    }

    void FixedUpdate()
    {
        if (!IsPaused())
        {
            AffectedByGravity();
            AffectedBySpin();

            //FreezeLocalVelocity();
        }
    }

    void LateUpdate()
    {
    }

    public void SetPlayer(GameObject stage, KeyManager keyManager, float moveSpeed, float rotSpeed, float jumpPower,
        float machineRadius, float spinSpeed, bool isSpiningCW)
    {
        key = keyManager;

        SetCreature(stage, moveSpeed, rotSpeed, jumpPower, machineRadius, spinSpeed, isSpiningCW);
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            rb.AddForce(transform.forward * 1000.0f);
    }

    void MovePlayer()
    {        
        //if (!_isJumping)
        {
            //rb.velocity += transform.forward * key.GetVerticalKey() * moveSpeed * 10.0f * Time.deltaTime;
            rb.position += transform.forward * key.GetVerticalKey() * moveSpeed * 0.8f * Time.deltaTime;

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

    public void MoveAlongStage(StageMovementValue values)
    {
        if (!IsPaused() && !GameManager.Instance.IsMachineStopped)
        {
            Vector3 centerforTurn = (values.isSwingRight) ? Vector3.left : -Vector3.left;
            Vector3 centerforSpin = (values.isSpiningCW) ? values.stageUpDir : -values.stageUpDir;
            Vector3 resPos = rb.position;

            Quaternion turnQuat = new Quaternion();
            Quaternion spinQuat = new Quaternion();

            // swing
            if (values.isSwinging)
                resPos += values.swingPosCur;

            // turn
            if (values.isTurning)
            {
                turnQuat = Quaternion.FromToRotation(transform.up, stage.transform.up);
                
                //turnQuat = Quaternion.AngleAxis(values.swingAngleCur, centerforTurn);
                //resPos = (turnQuat * (resPos - stage.transform.position) + stage.transform.position); //transform.RotateAround(stage.transform.position, Vector3.up, machineSpinSpeed * Time.fixedDeltaTime);
            }

            // spin
            if (values.isSpining && isOnStage)
            {
                spinQuat = Quaternion.AngleAxis(values.spinAngleCur, centerforSpin);
                resPos = (spinQuat * (resPos - stage.transform.position) + stage.transform.position);
            }

            // apply
            if (values.isSpining && isOnStage)
                rb.rotation = spinQuat * rb.rotation;

            if (values.isTurning && Mathf.Abs(values.stageX - transform.rotation.eulerAngles.x) > 0.01f)
                rb.rotation = turnQuat * rb.rotation;
                
            rb.position = resPos;
        }
    }

    void AffectedBySpin()
    {
        if (isOnStage && !GameManager.Instance.IsMachineStopped)
        {
            Vector3 dir = GetDirectionFromStageToCreature();

            rb.AddForce(dir * machineRadius * (55.5f + _spinSpeedUp) * Mathf.Deg2Rad * Time.fixedDeltaTime);

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

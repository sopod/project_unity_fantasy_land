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
            }
        }
    }
    
    public void SetPlayer(GameObject stage, KeyManager keyManager, float moveSpeed, float rotSpeed, float jumpPower,
                        float machineRadius, float spinSpeed, bool isSpiningCW)
    {
        key = keyManager;

        SetCreature(stage, moveSpeed, rotSpeed, jumpPower, machineRadius, spinSpeed, isSpiningCW);
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

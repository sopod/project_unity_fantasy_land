using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : LivingCreatures
{
    KeyController key;

    private void Update()
    {
        if (!IsPaused())
        {
            if (!IsStopped())
            {
                MovePlayer();
                TurnPlayer();
                JumpPlayer();

                DashPlayer();
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
    
    public void SetPlayer(GameObject stage, KeyController keyController, float machineRadius, Options options)
    {
        key = keyController;

        SetCreature(stage, machineRadius, options);
    }
    void DashPlayer()
    {
        if (key.IsDashKeyPressed())
            Dash();
    }

    void MovePlayer()
    {      
        Move(key.GetVerticalKey());
    }

    void TurnPlayer()
    {
        Turn(key.GetHorizontalKey());
    }

    void JumpPlayer()
    {
        if (key.IsJumpKeyPressed()) Jump();
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

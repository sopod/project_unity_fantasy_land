using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : LivingCreature
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
                FirePlayer();
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

        this.moveSpeed = options.PlayerMoveSpeed;
        this.rotSpeed = options.PlayerRotateSpeed;
        this.jumpPower = options.PlayerJumpPower;

        SetCreature(stage, machineRadius, options);
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
    
    void DashPlayer()
    {
        if (key.IsDashKeyPressed()) Dash();
    }

    void FirePlayer()
    {
        if (key.IsFireKeyPressed())
        {
            Fire();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        int layer = (1 << collision.gameObject.layer);

        if (layer == stageLayer.value)
        {
            isJumping = false;
            isOnStage = true;
            //isFlying = false;
            //isOnPlatform = false;

            ani.SetBool("IsJumping", false);
        }
        else if (layer == failZoneLayer.value)
        {
            //Debug.Log("You fell. Game Over.");

            isJumping = false;
            isOnStage = false;
            //isFlying = false;
            //isOnPlatform = false;
            isDead = true;

            ani.SetBool("IsDead", true);
            ani.SetBool("IsJumping", false);
        }
        //else if (layer == platformLayer.value)
        //{
        //    isJumping = false;
        //    isOnStage = true;
        //    isFlying = false;
        //    //isOnPlatform = true;

        //    ani.SetBool("IsJumping", false);
        //}
        else if (layer == enemyLayer.value)
        {

        }
        else
        {
            isOnStage = false;
            //isFlying = true;
            //isOnPlatform = false;
        }
    }


}

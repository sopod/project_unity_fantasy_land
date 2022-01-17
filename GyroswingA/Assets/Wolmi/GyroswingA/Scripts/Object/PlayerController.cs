using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public enum MoblieActionType
{
    Dash, 
    Jump, 
    Fire,
    Max
}



public class PlayerController : LivingCreature
{
    [SerializeField] JoystickController joystick;
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

                SetIsMovingAnimation();
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

    public void SetPlayer(GameObject stage, StageMovementValue stageVal, Options options)
    {
        key = new KeyController(joystick);

        this.creatureType = CreatureType.Player;
        this.moveSpeed = options.PlayerMoveSpeed;
        this.rotSpeed = options.PlayerRotateSpeed;
        this.jumpPower = options.PlayerJumpPower;

        SetCreature(stage, stageVal, options);
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
        if (key.IsJumpKeyPressed())
        {
            Jump();
        }
    }

    void DashPlayer()
    {
        if (key.IsDashKeyPressed())
        {
            Dash();
        }
    }

    void FirePlayer()
    {
        if (key.IsFireKeyPressed())
        {
            Fire();
        }
    }

    public void MobileButtonAction(MoblieActionType type)
    {
        if (type == MoblieActionType.Jump)
        {
            Jump();
        }
        else if (type == MoblieActionType.Dash)
        {
            Dash();
        }
        else if (type == MoblieActionType.Fire)
        {
            Fire();
        }
    }

    protected override void NotifyDead()
    {
        GameManager.Instance.SetFail();
    }

    // -------------------------------------------------- for item trigger enter
    void OnTriggerEnter(Collider other)
    {
        if (IsPaused()) return;

        int layer = (1 << other.gameObject.layer);

        if (layer == options.ItemLayer.value)
        {
            soundPlayer.PlaySound(CreatureEffectSoundType.ItemGet, IsPlayer);
        }
    }

    // -------------------------------------------------- for being damaged by enemy dash
    void OnCollisionStay(Collision collision) 
    {
        if (IsPaused()) return;

        int layer = (1 << collision.gameObject.layer);

        if (layer == options.EnemyLayer.value)
        {
            LivingCreature l = collision.gameObject.GetComponent<LivingCreature>();

            if (l.IsAttacking && !isDamaged)
            {
                OnDamagedAndMoveBack(false, false, l.CenterPosition, l.CenterForward);
            }
        }
    }

    // -------------------------------------------------- for layer collision
    void OnCollisionEnter(Collision collision)
    {
        if (IsPaused()) return;

        int layer = (1 << collision.gameObject.layer);

        if (layer == options.StageLayer.value)
        {
            OnStageLayer();
        }
        else if (layer == options.FailZoneLayer.value)
        {
            OnFailZoneLayer();
        }
        else if (layer == options.EnemyLayer.value)
        {
            OnEnemyLayer();
        }
        else
        {
            OnNothingLayer();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (IsPaused()) return;

        int layer = (1 << collision.gameObject.layer);

        if (layer == options.EnemyLayer.value)
        {
            isDamaged = false;
        }
        else if (layer == options.StageLayer.value)
        {
            //if (isJumping)
            //soundPlayer.PlaySound(CreatureEffectSoundType.Jump, IsPlayer);
        }
    }

    public override void OnEnemyLayer()
    {
        OnStageLayer();
    }
}

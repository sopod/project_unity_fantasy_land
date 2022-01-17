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
        key = new KeyController();

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

    public override void OnEnemyLayer()
    {

    }

    protected override void NotifyDead()
    {
        GameManager.Instance.SetFail();
    }
    void OnTriggerEnter(Collider other)
    {
        if (IsPaused()) return;

        int layer = (1 << other.gameObject.layer);

        if (layer == options.ItemLayer.value)
        {
            soundPlayer.PlaySound(CreatureEffectSoundType.ItemGet, IsPlayer);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (IsPaused()) return;

        int layer = (1 << collision.gameObject.layer);

        if (layer == options.EnemyLayer.value)
        {
            CheckDamagedToMoveBack(collision.gameObject.GetComponent<LivingCreature>());
        }
        else
        {
            //isDamaged = false;
        }

        if (layer == options.StageLayer.value)
        {
            OnStageLayer();
        }
    }

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
    }

}

using UnityEngine;

public enum MoblieActionType
{
    Dash, 
    Jump, 
    Fire,
    Max
}



public class Player : LivingCreature
{
    [SerializeField] JoystickController joystick;
    KeyController key;

    Vector3 playerStartPos = new Vector3(-30.57f, 2.21f, -50.2f);
    Quaternion playerStartRot;

    private void Update()
    {
        if (IsPaused) return;

        if (IsStopped) return;

        MovePlayer();
        TurnPlayer();
        JumpPlayer();

        DashPlayer();
        FirePlayer();

        SetIsMovingAnimation();
    }

    void FixedUpdate()
    {
        if (IsPaused) return;

        AffectedByPhysics();
    }

    public void SetPlayer(GameObject stage, StageMovementValue stageVal, LevelChanger options, Layers layer, ProjectileSpawner pjSpanwer)
    {
        values = SceneController.Instance.loaderGoogleSheet.ObjectDatas;

        playerStartRot = transform.rotation;

        key = new KeyController(joystick);

        creatureType = CreatureType.Player;
        curMoveSpeed = values.MoveSpeed;

        SetCreature(stage, stageVal, options, layer, pjSpanwer);
    }

    public override void ResetCreature()
    {
        base.ResetCreature();
        transform.position = playerStartPos;
        transform.rotation = playerStartRot;
        curMoveSpeed = values.MoveSpeed;
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
        if (key.IsFireKeyPressed()) Fire();
    }

    public void MobileButtonAction(MoblieActionType type)
    {
        if (type == MoblieActionType.Jump) Jump();
        else if (type == MoblieActionType.Dash) Dash();
        else if (type == MoblieActionType.Fire) Fire();
    }

    protected override void NotifyDead()
    {
        GameCenter.Instance.SetFail();
    }

    // -------------------------------------------------- item trigger enter
    void OnTriggerEnter(Collider other)
    {
        if (IsPaused) return;

        int layer = (1 << other.gameObject.layer);
        if (layer != layerStruct.ItemLayer.value) return;

        Item i = other.gameObject.GetComponent<Item>();
        ItemType type = (ItemType)i.Type;

        levelControl.OnPlayerSpeedItemUsed(type, values.MoveSpeed);

        float plusTime = levelControl.GetItemSecondsToAdd(type);
        GameCenter.Instance.OnItemUsed(plusTime);

        other.gameObject.SetActive(false);

        soundPlayer.PlaySound(CreatureEffectSoundType.ItemGet, IsPlayer);

        GameObject p = pjSpanwer.SpawnItemPickUpProjectile(this.gameObject);
        p.GetComponent<Projectile>().SetStart(pjSpanwer);
    }

    // -------------------------------------------------- damaged by enemy dash
    void OnCollisionStay(Collision collision) 
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);
        if (layer != layerStruct.EnemyLayer.value) return;

        Enemy e = collision.gameObject.GetComponent<Enemy>();
        if (e == null || !e.IsAttacking || isDamaged) return;

        GameObject p = pjSpanwer.SpawnDashHitProjectile(collision);
        p.GetComponent<Projectile>().SetStart(pjSpanwer);
        OnDamagedAndMoveBack(false, e.CenterPosition, e.CenterForward, (EnemyType)e.Type);
    }

    // -------------------------------------------------- layer collision
    void OnCollisionEnter(Collision collision)
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);

        if (layer == layerStruct.StageLayer.value)
        {
            OnStageLayer();
        }
        else if (layer == layerStruct.FailZoneLayer.value)
        {
            OnFailZoneLayer();
        }
        else if (layer == layerStruct.EnemyLayer.value)
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
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);
        if (layer != layerStruct.EnemyLayer.value) return;

        isDamaged = false;
    }

    public override void OnEnemyLayer()
    {
        OnStageLayer();
    }
}

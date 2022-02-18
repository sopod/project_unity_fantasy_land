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

        aniPlay.DoMovingAnimation();
    }

    void FixedUpdate()
    {
        if (IsPaused) return;

        AffectedByPhysics();
    }

    public void InitPlayer(GameObject stage, StageMovementValue stageVal, StageChanger options, Layers layer, ProjectileSpawner pjSpanwer)
    {
        values = SceneController.Instance.loaderGoogleSheet.ObjectDatas;

        playerStartRot = transform.rotation;

        key = new KeyController(joystick);

        creatureType = CreatureType.Player;
        curMoveSpeed = values.MoveSpeed;

        Init(stage, stageVal, layer, pjSpanwer);
    }

    public override void ResetValues()
    {
        base.ResetValues();
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
        GameCenter.Instance.OnFail();
    }

    protected override void OnDamagedByDash(Collision collision)
    {
        Enemy e = collision.gameObject.GetComponent<Enemy>();
        if (e == null || !e.IsAttacking || isDamaged) return;

        if (IsHitBack(e.CenterPosition, e.CenterForward)) return;

        Vector3 dir = (transform.position - collision.transform.position).normalized;
        float damage = values.GetDashPowerToDamagedByEnemy((EnemyType)e.Type, values.DashPowerToDamaged);
        TakeDamage(dir, damage);

        pjSpanwer.SpawnDashHitProjectile(collision);
    }

    
    // -------------------------------------------------- item trigger enter
    void OnTriggerEnter(Collider other)
    {
        if (IsPaused) return;

        int layer = (1 << other.gameObject.layer);
        if (layer != layers.ItemLayer.value) return;

        Item i = other.gameObject.GetComponent<Item>();
        ItemType type = (ItemType)i.Type;

        curMoveSpeed = values.GetSpeedToUpgradeByItem(type, values.MoveSpeed);
        float plusTime = values.GetTimeToUpgradeByItem(type);
        GameCenter.Instance.OnItemGot(plusTime);

        soundPlay.DoItemGetSound(IsPlayer);
        pjSpanwer.SpawnItemPickUpProjectile(this.gameObject);
        i.InvokeBackToPool();
    }

    // -------------------------------------------------- damaged by enemy dash
    void OnCollisionStay(Collision collision) 
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);
        if (layer != layers.EnemyLayer.value) return;

        OnDamagedByDash(collision);
    }

    // -------------------------------------------------- layer collision
    void OnCollisionEnter(Collision collision)
    {
        if (IsPaused) return;
        int layer = (1 << collision.gameObject.layer);

        if (layer == layers.StageLayer.value || layer == layers.EnemyLayer.value)
        {
            OnStageLayer();
        }
        else if (layer == layers.FailZoneLayer.value)
        {
            OnFailZoneLayer();
        }
        else
        {
            //OnNothingLayer();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (IsPaused) return;

        int layer = (1 << collision.gameObject.layer);
        if (layer != layers.EnemyLayer.value) return;

        isDamaged = false;
    }

}

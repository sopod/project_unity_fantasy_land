using System;
using UnityEngine;
using UnityEngine.UI;

public enum CreatureType
{
    Enemy, 
    Player,
    Max
}


public abstract class LivingCreature : MovingThing
{
    const float GRAVITY = 9.8f;
    const float FALL_GRAVITY = GRAVITY * 2f;

    public Action OnDead;

    protected Rigidbody rb;
    protected State state = new State();
    protected Status status = new Status();
    protected CreatureAnimation aniPlay;
    protected CreatureSound soundPlay;

    protected CreatureType creatureType;
    public bool IsPlayer { get { return creatureType == CreatureType.Player; } }

    protected float curMoveSpeed;

    [SerializeField] GameObject centerOfCreature;
    [SerializeField] GameObject shootMouth;
    [SerializeField] Billboard hpBar;
    [SerializeField] Slider hpSlider;

    protected ObjectValues values;
    protected ProjectileSpawner pjSpanwer;
    protected Layers layers;
    StageMovementValue stageVal;
    GameObject stageOfMachine;

    public Vector3 CenterForward { get => centerOfCreature.transform.forward; }
    public Vector3 CenterPosition { get => centerOfCreature.transform.position; }
    public bool IsAttacking { get => state.IsAttacking; }


    protected virtual void Init(GameObject stageOfMachine, StageMovementValue stageVal,
        Layers layers, ProjectileSpawner pjSpanwer, Transform playerCamera)
    {
        values = SceneController.Instance.loaderGoogleSheet.ObjectDatas;
        this.stageVal = stageVal;
        this.layers = layers;
        this.pjSpanwer = pjSpanwer;

        hpBar.camera = playerCamera;

        this.stageOfMachine = stageOfMachine;
        aniPlay = new CreatureAnimation(GetComponent<Animator>());
        rb = GetComponent<Rigidbody>();
        soundPlay = new CreatureSound(GetComponentInChildren<CreatureSoundPlayer>());

        state.SetIdle();

        rb.useGravity = false;
        rb.mass = 1;
        rb.drag = 5;
        rb.angularDrag = 20;
        rb.constraints = RigidbodyConstraints.FreezeRotationY;

        ResetValues();
    }

    public virtual void ResetValues()
    {
        state.InitState();
        status.InitStatus();
        hpSlider.value = status.HealthSilderValue;
        aniPlay.InitAnimation();
        PauseMoving();
    }
    
    protected void AffectedByPhysics()
    {
        AffectedByGravity();
        FreezeLocalXZRotation();
    }

    protected void AffectedByGravity()
    {
        if (state.IsInStageBoundary) rb.velocity -= stageOfMachine.transform.up * GRAVITY * Time.deltaTime;
        else rb.velocity -= Vector3.up * FALL_GRAVITY * Time.deltaTime;
    }

    protected void FreezeLocalXZRotation()
    {
        if (!state.CanMoveAlongWithMachine) return;

        Quaternion turnQuat = Quaternion.FromToRotation(centerOfCreature.transform.up, stageOfMachine.transform.up);
        rb.rotation = turnQuat * rb.rotation;
    }

    public void Move(float key)
    {
        if (!state.CanMove) return;

        aniPlay.SetMoveAnimation(key);

        rb.position += transform.forward * key * curMoveSpeed * Time.deltaTime;
    }

    public void Turn(float key)
    {
        if (!state.CanMove) return;

        aniPlay.SetTurnAnimation(key);

        float angle = key * values.RotateSpeed * Time.deltaTime;
        rb.rotation *= Quaternion.AngleAxis(angle, Vector3.up);
    }

    public void Jump()
    {
        if (!state.CanJump) return;

        state.SetJumping();

        soundPlay.DoJumpSound(IsPlayer);
        aniPlay.DoJumpAnimation();

        rb.AddForce(stageOfMachine.transform.up * values.JumpPower, ForceMode.Impulse);
    }

    public void JumpByAttack()
    {
        rb.AddForce(stageOfMachine.transform.up * values.JumpPower, ForceMode.Impulse);
    }

    public void Dash()
    {
        if (!state.CanAttack) return;

        state.SetAttacking();
        soundPlay.DoDashSound(IsPlayer);
        aniPlay.DoDashAnimation();

        rb.AddForce(transform.forward * values.DashPowerToHit, ForceMode.Impulse);

        CancelInvoke("SetIdle");
        Invoke("SetIdle", values.SkillCoolTime);
    }

    public void Fire()
    {
        if (!state.CanAttack) return;

        state.SetAttacking();
        soundPlay.DofireSound(IsPlayer);
        aniPlay.DoFireAnimation();

        pjSpanwer.SpawnFireProjectile(shootMouth.transform.position, shootMouth.transform.forward);

        CancelInvoke("SetIdle");
        Invoke("SetIdle", values.SkillCoolTime);
    }

    void SetIdle()
    {
        state.SetIdle();
    }

    protected bool IsHitBack(Vector3 centerPosOfAttacker, Vector3 forwardOfAttacker)
    {
        Vector2 targetPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 attackerPos = new Vector2(centerPosOfAttacker.x, centerPosOfAttacker.z);
        Vector2 lookTargetDir = targetPos - attackerPos;

        Vector2 attackedForward = new Vector2(forwardOfAttacker.x, forwardOfAttacker.z);

        return (attackedForward.x * lookTargetDir.x + attackedForward.y * lookTargetDir.y < 0);
    }

    protected void TakeDamage(Vector3 dir, float damagedPower)
    {
        state.SetDamaged();
        
        JumpByAttack();

        rb.AddForce(dir * damagedPower, ForceMode.Impulse);
        soundPlay.DoHitSound(IsPlayer);

        status.ReduceHealth(damagedPower);
        hpSlider.value = status.HealthSilderValue;
        if (status.HealthSilderValue == 0.0f)
        {
            OnDeadProcess();
            return;
        }

        CancelInvoke("SetIdle");
        Invoke("SetIdle", values.SkillCoolTime);
    }

    protected abstract void OnDamagedByDash(Collision collision);

    public void MoveAlongWithStage()
    {
        if (IsPaused || !state.CanMoveAlongWithMachine) return;

        Vector3 centerForSpin = (stageVal.IsSpiningCW) ? stageOfMachine.transform.up : -stageOfMachine.transform.up;
        Vector3 resPos = rb.position;

        if (stageVal.IsMachineSwinging)
            resPos += stageVal.SwingPosCur;

        if (stageVal.IsMachineSpining && state.IsOnJumpableObject)
        {
            Quaternion spinQuat = Quaternion.AngleAxis(stageVal.SpinAngleCur, centerForSpin);
            resPos = (spinQuat * (resPos - stageOfMachine.transform.position) + stageOfMachine.transform.position);
            rb.rotation = spinQuat * rb.rotation;
        }

        rb.position = resPos;
    }

    protected void OnStageLayer()
    {
        state.IsOnJumpableObject = true;
        state.IsInStageBoundary = true;

        if (state.IsJumping)
        {
            state.SetIdle();
            aniPlay.EndJumpAnimation();
        }
    }

    protected void OffStageLayer()
    {
        state.IsOnJumpableObject = false;
    }

    public void OnFailZoneLayer()
    {
        if (state.IsDead) return;

        OnDeadProcess();
    }

    protected virtual void OnDeadProcess()
    {
        state.SetDead();

        soundPlay.DoDeadSound(true);
        aniPlay.DoDeadAnimation();

        pjSpanwer.SpawnDeadProjectile(gameObject);

        OnDead?.Invoke();
    }
}

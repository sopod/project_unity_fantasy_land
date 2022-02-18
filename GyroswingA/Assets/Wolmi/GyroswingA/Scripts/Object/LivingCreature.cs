using UnityEngine;


public enum CreatureType
{
    Enemy, 
    Player,
    Max
}

// 적 몬스터와 플레이어 오브젝트가 상속하는 LivingCreature 클래스입니다. 


public abstract class LivingCreature : MovingThing
{
    const float gravity = 9.8f;

    Rigidbody rb;
    [SerializeField] protected State state = new State();
    protected CreatureAnimation aniPlay;
    protected CreatureSound soundPlay;

    protected CreatureType creatureType;
    public bool IsPlayer { get { return creatureType == CreatureType.Player; } }

    bool isInStageBoundary = true;
    [SerializeField] bool isOnJumpableObject = true;
    [SerializeField] protected bool isDamaged = false;

    protected float curMoveSpeed;
    //protected float spinSpeedUp;

    [SerializeField] GameObject centerOfCreature;
    [SerializeField] GameObject shootMouth;

    protected ObjectValues values;
    protected ProjectileSpawner pjSpanwer;
    protected Layers layers;
    StageMovementValue stageVal;
    GameObject stageOfMachine;

    public Vector3 CenterForward { get => centerOfCreature.transform.forward; }
    public Vector3 CenterPosition { get => centerOfCreature.transform.position; }
    public bool IsAttacking { get => state.IsAttacking; }


    protected virtual void Init(GameObject stageOfMachine, StageMovementValue stageVal, Layers layers, ProjectileSpawner pjSpanwer)
    {
        this.stageVal = stageVal;
        this.layers = layers;
        this.pjSpanwer = pjSpanwer;

        this.stageOfMachine = stageOfMachine;
        aniPlay = new CreatureAnimation(GetComponent<Animator>());
        rb = GetComponent<Rigidbody>();
        soundPlay = new CreatureSound(GetComponentInChildren<CreatureSoundPlayer>());

        state.SetIdle();

        rb.useGravity = false;
        rb.mass = 1;
        rb.drag = 5;
        rb.angularDrag = 20;
        
        //spinSpeedUp = 0.0f;

        ResetValues();
    }

    public virtual void ResetValues()
    {
        state.SetIdle();
        aniPlay.InitAnimation();

        isOnJumpableObject = true;
        //isInStageBoundary = true;

        //isJumping = false;
        isDamaged = false;
        
        PauseMoving();
    }
    
    protected void AffectedByPhysics()
    {
        AffectedByGravity();
        //AffectedBySpin();
        FreezeLocalXZRotation();
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
        if (!state.CanJump && !isOnJumpableObject) return;

        state.SetJumping();
        isOnJumpableObject = false;

        soundPlay.DoJumpSound(IsPlayer);
        aniPlay.DoJumpAnimation();

        rb.AddForce(stageOfMachine.transform.up * values.JumpPower, ForceMode.Impulse);
    }

    public void JumpByAttack()
    {
        isOnJumpableObject = false;

        state.SetJumping();
        aniPlay.DoJumpAnimation();

        rb.AddForce(stageOfMachine.transform.up * values.JumpPower, ForceMode.Impulse);
    }

    public void Dash()
    {
        if (!state.CanAttack) return;

        state.SetAttacking();
        soundPlay.DoDashSound(IsPlayer);
        aniPlay.DoDashAnimation();

        rb.AddForce(transform.forward * values.DashPowerToHit, ForceMode.Impulse);
        
        Invoke("SetIdle", values.SkillCoolTime);
    }

    public void Fire()
    {
        if (!state.CanAttack) return;

        state.SetAttacking();
        soundPlay.DofireSound(IsPlayer);
        aniPlay.DoFireAnimation();

        pjSpanwer.SpawnFireProjectile(shootMouth.transform.position, shootMouth.transform.forward);
        
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
        if (!IsPlayer) JumpByAttack();

        isDamaged = true;

        rb.AddForce(dir * damagedPower, ForceMode.Impulse);
        soundPlay.DoHitSound(IsPlayer);

        Invoke("AcceptDamaged", values.SkillCoolTime);
    }

    protected abstract void OnDamagedByDash(Collision collision);

    //protected void OnDamagedAndMoveBack(Vector3 dir, EnemyType type, bool isProjectile)
    //{
    //    isDamaged = true;

    //    float damagedPower = values.DashPowerToDamaged;

    //    if (IsPlayer && !isProjectile) // this is player script and player damaged
    //    {
    //        damagedPower = values.GetDashPowerToDamagedByEnemy(type, values.DashPowerToDamaged);
    //    }
    //    else if (isProjectile) // this is enemy script and enemy damaged by projectile
    //    {
    //        damagedPower = values.FireBallPowerToDamaged;
    //    }

    //    TakeDamage(dir, damagedPower);
    //}

    public void AcceptDamaged()
    {
        isDamaged = false;
    }

    protected abstract void NotifyDead();

    protected void AffectedByGravity()
    {
        rb.velocity -= stageOfMachine.transform.up * gravity * Time.deltaTime;
    }

    //protected void AffectedBySpin()
    //{
    //    if (!isOnJumpableObject || !isInStageBoundary) return;

    //    Vector3 dir = GetDirectionFromStageToCreature();

    //    rb.velocity += (dir * stageVal.Radius * (55.5f + spinSpeedUp) * Mathf.Deg2Rad * Time.deltaTime);
    //}

    public void MoveAlongWithStage(bool isMachineSwinging, bool isMachineSpining, bool isSpiningCW)
    {
        if (IsPaused || !state.CanMoveAlongWithMachine) return;

        Vector3 centerForSpin = (isSpiningCW) ? stageOfMachine.transform.up : -stageOfMachine.transform.up;
        Vector3 resPos = rb.position;

        // swing
        if (isMachineSwinging)
            resPos += stageVal.SwingPosCur;

        // spin
        if (isMachineSpining && isOnJumpableObject)
        {
            Quaternion spinQuat = Quaternion.AngleAxis(stageVal.SpinAngleCur, centerForSpin);
            resPos = (spinQuat * (resPos - stageOfMachine.transform.position) + stageOfMachine.transform.position);
            rb.rotation = spinQuat * rb.rotation;
        }

        // apply
        rb.position = resPos;
    }

    protected void FreezeLocalXZRotation()
    {
        if (!state.CanMoveAlongWithMachine) return;

        Quaternion turnQuat = Quaternion.FromToRotation(centerOfCreature.transform.up, stageOfMachine.transform.up);
        rb.rotation = turnQuat * rb.rotation;
    }

    //protected Vector3 GetDirectionFromStageToCreature()
    //{
    //    Vector3 centerPosOfCreature = centerOfCreature.GetComponent<Renderer>().bounds.center;
    //    Vector3 fromStageToCreature = centerPosOfCreature - stageOfMachine.transform.position;
    //    float height = Vector3.Dot(fromStageToCreature, stageOfMachine.transform.up.normalized);

    //    Vector3 parallelPos = stageOfMachine.transform.position + new Vector3(0, height, 0);
    //    Vector3 res = (centerPosOfCreature - parallelPos).normalized;

    //    //Debug.DrawRay(centerPosOfCreature, res, Color.red);

    //    return res;
    //}

    // -------------------------------------------------- move along with stage
    //void OnTriggerEnter(Collider other)
    //{
    //    if (IsPaused) return;
            
    //    int layer = (1 << other.gameObject.layer);
    //    if (layer != layers.StageBoundaryLayer.value)

    //    isInStageBoundary = true;
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (IsPaused) return;

    //    int layer = (1 << other.gameObject.layer);
    //    if (layer != layers.StageBoundaryLayer.value) return;

    //    isInStageBoundary = false;
    //}

    // -------------------------------------------------- layer collision
    public void OnStageLayer()
    {
        //isInStageBoundary = true;

        isOnJumpableObject = true;

        if (state.IsJumping)
        {
            state.SetIdle();
            aniPlay.EndJumpAnimation();
        }
    }

    public void OnFailZoneLayer()
    {
        if (state.IsDead) return;

        Debug.Log("SetDead");
        state.SetDead();

        isOnJumpableObject = false;
        
        soundPlay.DoDeadSound(true);
        aniPlay.DoDeadAnimation();

        pjSpanwer.SpawnDeadProjectile(this.gameObject);

        NotifyDead();
    }
    
    public void OnNothingLayer()
    {
        isOnJumpableObject = false;
    }
}

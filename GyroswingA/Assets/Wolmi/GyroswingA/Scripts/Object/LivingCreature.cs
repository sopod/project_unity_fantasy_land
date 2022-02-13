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
    Animator ani;
    State state = new State();

    protected CreatureType creatureType;
    public bool IsPlayer { get { return creatureType == CreatureType.Player; } }
        
    bool isOnJumpableObject = true;
    bool isInStageBoundary = true;
    [SerializeField] protected bool isJumping = false;
    [SerializeField] protected bool isMoving = false;
    [SerializeField] protected bool isTurning = false;
    [SerializeField] protected bool isDamaged = false;

    protected float curMoveSpeed;
    //protected float spinSpeedUp;

    protected CreatureSoundPlayer soundPlayer;
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
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        soundPlayer = GetComponentInChildren<CreatureSoundPlayer>();

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
        InitAnimation();

        isOnJumpableObject = true;
        isInStageBoundary = true;

        isJumping = false;
        isMoving = false;
        isTurning = false;
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
        if (state.IsAttacking) return;

        if ((Mathf.Abs(key) > 0.1f)) isMoving = true;
        else isMoving = false;

        ani.SetFloat("MoveFront", key);

        rb.position += transform.forward * key * curMoveSpeed * Time.deltaTime;
    }

    public void Turn(float key)
    {
        if (state.IsAttacking) return;

        if ((Mathf.Abs(key) > 0.1f)) isTurning = true;
        else isTurning = false;

        ani.SetFloat("TurnRight", key);

        float angle = key * values.RotateSpeed * Time.deltaTime;
        rb.rotation *= Quaternion.AngleAxis(angle, Vector3.up);
    }

    public void Jump()
    {
        if (isJumping || state.IsAttacking || !isOnJumpableObject) return;

        isJumping = true;
        isOnJumpableObject = false;

        soundPlayer.PlaySound(CreatureEffectSoundType.Jump, IsPlayer);
        ani.SetBool("IsJumping", true);

        rb.AddForce(stageOfMachine.transform.up * values.JumpPower, ForceMode.Impulse);
    }

    public void JumpByAttack()
    {
        isJumping = true;
        isOnJumpableObject = false;

        ani.SetBool("IsJumping", true);

        rb.AddForce(stageOfMachine.transform.up * values.JumpPower, ForceMode.Impulse);
    }

    public void Dash()
    {
        if (isJumping || state.IsAttacking) return;

        soundPlayer.PlaySound(CreatureEffectSoundType.Dash, IsPlayer);

        ani.SetTrigger("JustDashed");
        rb.AddForce(transform.forward * values.DashPowerToHit, ForceMode.Impulse);
        
        state.SetAttacking();
        
        Invoke("SetIdle", values.SkillCoolTime);
    }

    public void Fire()
    {
        if (isJumping || state.IsAttacking) return;
        
        soundPlayer.PlaySound(CreatureEffectSoundType.Fire, IsPlayer);
        ani.SetTrigger("JustFired");


        GameObject p = pjSpanwer.SpawnFireProjectile(shootMouth.transform.position, shootMouth.transform.forward);
        p.GetComponent<Projectile>().SetStart(pjSpanwer);

        state.SetAttacking();
        
        Invoke("SetIdle", values.SkillCoolTime);
    }

    protected void OnDamagedAndMoveBack(bool isProjectile, Vector3 centerPosOfAttacker, Vector3 forwardPosOfAttacker, EnemyType type)
    {
        isDamaged = true;

        if (!isProjectile)
        {
            // return, if it's back
            Vector2 targetPos = new Vector2(this.transform.position.x, this.transform.position.z);
            Vector2 attackerPos = new Vector2(centerPosOfAttacker.x, centerPosOfAttacker.z);
            Vector2 attackedForward = new Vector2(forwardPosOfAttacker.x, forwardPosOfAttacker.z);

            Vector2 lookTargetDir = targetPos - attackerPos;
            if (attackedForward.x * lookTargetDir.x + attackedForward.y * lookTargetDir.y < 0) return;
        }

        Vector3 dir = (CenterPosition - centerPosOfAttacker).normalized;
        float damagedPower = values.DashPowerToDamaged;

        if (IsPlayer && type != EnemyType.Max) // this is player script and player damaged
        {
            damagedPower = values.GetDashPowerToDamagedByEnemy(type, values.DashPowerToDamaged);
        }
        else if (isProjectile) // this is enemy script and enemy damaged by projectile
        {
            damagedPower = values.FireBallPowerToDamaged;
        }

        if (!IsPlayer) // this is enemy script and enemy damaged
        {
            soundPlayer.PlaySound(CreatureEffectSoundType.Dash, IsPlayer);
            JumpByAttack();
        }

        // add force by dash power
        rb.AddForce(dir * damagedPower, ForceMode.Impulse);

        Invoke("AcceptDamaged", values.SkillCoolTime);
    }

    public void SetIdle()
    {
        state.SetIdle();
    }

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
        if (IsPaused || !isInStageBoundary) return;

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
        if (!isInStageBoundary && !isJumping) return;

        Quaternion turnQuat = Quaternion.FromToRotation(centerOfCreature.transform.up, stageOfMachine.transform.up);
        rb.rotation = turnQuat * rb.rotation;
    }

    protected Vector3 GetDirectionFromStageToCreature()
    {
        Vector3 centerPosOfCreature = centerOfCreature.GetComponent<Renderer>().bounds.center;
        Vector3 fromStageToCreature = centerPosOfCreature - stageOfMachine.transform.position;
        float height = Vector3.Dot(fromStageToCreature, stageOfMachine.transform.up.normalized);

        Vector3 parallelPos = stageOfMachine.transform.position + new Vector3(0, height, 0);
        Vector3 res = (centerPosOfCreature - parallelPos).normalized;

        //Debug.DrawRay(centerPosOfCreature, res, Color.red);

        return res;
    }

    public void InitAnimation()
    {
        state.SetIdle();

        ani.SetFloat("MoveFront", 0.0f);
        ani.SetFloat("TurnRight", 0.0f);
        ani.SetBool("IsMoving", false);
        ani.SetBool("IsJumping", false);
        ani.SetBool("IsDead", false);
    }

    protected void SetIsMovingAnimation()
    {
        ani.SetBool("IsMoving", isMoving || isTurning);
    }

    // -------------------------------------------------- move along with stage
    void OnTriggerEnter(Collider other)
    {
        if (IsPaused) return;
            
        int layer = (1 << other.gameObject.layer);
        if (layer != layers.StageBoundaryLayer.value)

        isInStageBoundary = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPaused) return;

        int layer = (1 << other.gameObject.layer);
        if (layer != layers.StageBoundaryLayer.value) return;

        isInStageBoundary = false;
    }

    // -------------------------------------------------- layer collision
    public void OnStageLayer()
    {
        isInStageBoundary = true;

        isJumping = false;
        isOnJumpableObject = true;
        
        ani.SetBool("IsJumping", false);
    }

    public void OnFailZoneLayer()
    {
        if (state.IsDead) return;

        soundPlayer.PlaySound(CreatureEffectSoundType.Dead, IsPlayer);

        isJumping = false;
        isOnJumpableObject = false;
        state.SetDead();
        
        NotifyDead();

        ani.SetBool("IsDead", true);
        ani.SetBool("IsJumping", false);

        GameObject p = pjSpanwer.SpawnDeadProjectile(this.gameObject);
        p.GetComponent<Projectile>().SetStart(pjSpanwer);
    }

    public abstract void OnEnemyLayer();

    public void OnNothingLayer()
    {
        isOnJumpableObject = false;
    }
}

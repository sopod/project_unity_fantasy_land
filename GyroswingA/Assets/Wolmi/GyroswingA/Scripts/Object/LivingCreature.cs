using UnityEngine;

public enum CreatureType
{
    Enemy, 
    Player,
    Max
}



public abstract class LivingCreature : MovingThing
{
    protected Options options;
    protected StageMovementValue stageVal;

    protected GameObject stage;
    protected CreatureSoundPlayer soundPlayer;
    [SerializeField] protected GameObject centerOfCreature;
    [SerializeField] protected GameObject shootMouth;

    protected Rigidbody rb;
    protected Animator ani;

    protected CreatureType creatureType;
    public bool IsPlayer
    {
        get { return creatureType == CreatureType.Player; }
    }

    protected StateController state;

    [SerializeField] protected bool isJumping;
    [SerializeField] protected bool isOnJumpableObject;

    [SerializeField] protected bool isMoving;
    [SerializeField] protected bool isTurning;
    [SerializeField] protected bool isDamaged;
    [SerializeField] protected bool isInStageBoundary;


    protected float moveSpeed;
    protected float rotSpeed;
    protected float jumpPower;
    
    protected float _spinSpeedUp;

    public Vector3 CenterForward { get { return centerOfCreature.transform.forward; } }

    public Vector3 CenterPosition { get { return centerOfCreature.transform.position; } }
    public bool IsAttacking { get { return state.IsAttacking; } }



    protected void SetCreature(GameObject stage, StageMovementValue stageVal, Options options)
    {
        this.options = options;
        this.stageVal = stageVal;

        this.stage = stage;
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        soundPlayer = GetComponentInChildren<CreatureSoundPlayer>();

        state = new StateController();

        rb.useGravity = false;
        rb.mass = 1;
        rb.drag = 5;
        rb.angularDrag = 10;
        
        _spinSpeedUp = 0.0f;

        ResetCreature();
    }

    public void ResetCreature()
    {
        InitAnimation();

        isMoving = false;
        isTurning = false;

        isJumping = false;
        isOnJumpableObject = true;

        isInStageBoundary = true;
        isDamaged = false;

        PauseMoving();
    }
    
    protected void AffectedByPhysics()
    {
        AffectedByGravity();
        //AffectedBySpin();
        FreezeLocalXZRotation();
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

    public void Move(float key)
    {
        if (state.IsAttacking) return;

        if ((Mathf.Abs(key) > 0.1f))
            isMoving = true;
        else
            isMoving = false;

        ani.SetFloat("MoveFront", key);

        rb.position += transform.forward * key * moveSpeed * Time.deltaTime;
    }

    public void Turn(float key)
    {
        if (state.IsAttacking) return;

        if ((Mathf.Abs(key) > 0.1f))
            isTurning = true;
        else
            isTurning = false;

        ani.SetFloat("TurnRight", key);

        float angle = key * rotSpeed * Time.deltaTime;
        rb.rotation *= Quaternion.AngleAxis(angle, Vector3.up);
    }

    public void Jump()
    {
        if (isJumping || state.IsAttacking || !isOnJumpableObject) return;

        isJumping = true;
        isOnJumpableObject = false;

        soundPlayer.PlaySound(CreatureEffectSoundType.Jump, IsPlayer);
        ani.SetBool("IsJumping", true);

        rb.AddForce(stage.transform.up * jumpPower, ForceMode.Impulse);
    }

    public void Dash()
    {
        if (isJumping || state.IsAttacking) return;

        soundPlayer.PlaySound(CreatureEffectSoundType.Dash, IsPlayer);

        ani.SetTrigger("JustDashed");
        rb.AddForce(transform.forward * options.DashPowerToHit, ForceMode.Impulse);
        
        state.SetAttacking();
        
        Invoke("SetIdle", options.SkillCoolTime);
    }

    public void Fire()
    {
        if (isJumping || state.IsAttacking) return;
        
        soundPlayer.PlaySound(CreatureEffectSoundType.Fire, IsPlayer);
        ani.SetTrigger("JustFired");

        GameManager.Instance.SpawnProjectile(shootMouth); // fire

        state.SetAttacking();
        
        Invoke("SetIdle", options.SkillCoolTime);
    }

    protected void OnDamagedAndMoveBack(bool isAttackedByPlayer, bool isProjectile, Vector3 centerPosOfAttacker, Vector3 forwardPosOfAttacker, EnemyType type)
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


        // give dash power
        Vector3 dir = (CenterPosition - centerPosOfAttacker).normalized;
        float damagedPower = options.DashPowerToDamaged;

        if (isAttackedByPlayer && type != EnemyType.Max)
        {
            soundPlayer.PlaySound(CreatureEffectSoundType.Dash, IsPlayer);
            
            damagedPower = options.GetDashPowerToDamaged(type);
        }

        if (isProjectile)
        {
            damagedPower = options.FireBallPowerToDamaged;
        }

        rb.AddForce(dir * damagedPower, ForceMode.Impulse);
    }

    public void SetIdle()
    {
        state.SetIdle();
    }

    protected abstract void NotifyDead();

    protected void AffectedByGravity()
    {
        rb.velocity -= stage.transform.up * options.Gravity * Time.deltaTime;
    }

    protected void AffectedBySpin()
    {
        if (isOnJumpableObject && !GameManager.Instance.IsMachineStopped && isInStageBoundary)
        {
            Vector3 dir = GetDirectionFromStageToCreature();

            rb.velocity += (dir * stageVal.Radius * (55.5f + _spinSpeedUp) * Mathf.Deg2Rad * Time.deltaTime);
        }
    }

    public void MoveAlongWithStage()
    {
        if (!IsPaused() && !GameManager.Instance.IsMachineStopped && isInStageBoundary)
        {
            Vector3 centerForSpin = (options.IsSpiningCW) ? stage.transform.up : -stage.transform.up;
            Vector3 resPos = rb.position;

            // swing
            if (options.IsMachineSwinging)
                resPos += stageVal.SwingPosCur;

            // spin
            if (options.IsMachineSpining && isOnJumpableObject && !isJumping)
            {
                Quaternion spinQuat = Quaternion.AngleAxis(stageVal.SpinAngleCur, centerForSpin);
                resPos = (spinQuat * (resPos - stage.transform.position) + stage.transform.position);
                rb.rotation = spinQuat * rb.rotation;
            }

            // apply
            rb.position = resPos;
        }
    }

    protected void FreezeLocalXZRotation()
    {
        if (isInStageBoundary)
        {
            Quaternion turnQuat = Quaternion.FromToRotation(centerOfCreature.transform.up, stage.transform.up);
            rb.rotation = turnQuat * rb.rotation;
        }
    }

    protected Vector3 GetDirectionFromStageToCreature()
    {
        Vector3 centerPosOfCreature = centerOfCreature.GetComponent<Renderer>().bounds.center;
        Vector3 fromStageToCreature = centerPosOfCreature - stage.transform.position;
        float height = Vector3.Dot(fromStageToCreature, stage.transform.up.normalized);

        Vector3 parallelPos = stage.transform.position + new Vector3(0, height, 0);
        Vector3 res = (centerPosOfCreature - parallelPos).normalized;

        Debug.DrawRay(centerPosOfCreature, res, Color.red);

        return res;
    }


    // -------------------------------------------------- for move along with stage
    void OnTriggerEnter(Collider other)
    {
        if (IsPaused()) return;
            
        int layer = (1 << other.gameObject.layer);

        if (layer == options.StageBoundaryLayer.value)
        {
            isInStageBoundary = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (IsPaused()) return;

        int layer = (1 << other.gameObject.layer);

        if (layer == options.StageBoundaryLayer.value)
        {
            isInStageBoundary = false;
        }
    }

    // -------------------------------------------------- for layer collision
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
    }


    public abstract void OnEnemyLayer();

    public void OnNothingLayer()
    {
        isOnJumpableObject = false;
    }
    

}

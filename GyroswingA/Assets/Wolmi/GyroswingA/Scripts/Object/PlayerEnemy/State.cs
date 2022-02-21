public enum CreatureState
{
    Idle,
    Jumping,
    Attacking,
    Damaged,
    Dead,
    Max
}

public class State
{
    const float JUMP_COOLTIME = 0.5f;
    StopWatch stateTimer = new StopWatch();

    CreatureState state = CreatureState.Idle;

    public bool IsOnJumpableObject = true;
    public bool IsInStageBoundary = true;

    public bool CanMove                 { get => !IsAttacking; }
    public bool CanJump                 { get => (!IsJumping && !IsAttacking && IsOnJumpableObject 
                                                  && !IsDamaged && (!stateTimer.HasStarted || stateTimer.IsFinished)); }
    public bool CanAttack               { get => (!IsJumping && !IsAttacking && !IsDamaged); }
    public bool CanMoveAlongWithMachine { get => (IsInStageBoundary); }

    public bool IsIdle                  { get => (state == CreatureState.Idle); }
    public bool IsJumping               { get => (state == CreatureState.Jumping); }
    public bool IsAttacking             { get => (state == CreatureState.Attacking); }
    public bool IsDamaged               { get => (state == CreatureState.Damaged); }
    public bool IsDead                  { get => (state == CreatureState.Dead); }


    public void InitState()
    {
        SetIdle();
        IsOnJumpableObject = true;
        IsInStageBoundary = true;
    }

    public CreatureState GetCurrentState()
    {
        return state;
    }

    void ChangeState(CreatureState inState)
    {
        state = inState;
    }

    public void SetIdle()
    {
        ChangeState(CreatureState.Idle);
    }

    public void SetJumping()
    {
        IsOnJumpableObject = false;
        ChangeState(CreatureState.Jumping);
        stateTimer.StartTimer(JUMP_COOLTIME);
    }

    public void SetAttacking()
    {
        ChangeState(CreatureState.Attacking);
    }

    public void SetDamaged()
    {
        IsOnJumpableObject = false;
        ChangeState(CreatureState.Damaged);
    }

    public void SetDead()
    {
        IsOnJumpableObject = false;
        ChangeState(CreatureState.Dead);
    }
}

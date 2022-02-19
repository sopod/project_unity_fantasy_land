using UnityEngine;

public enum CreatureState
{
    Idle,
    Jumping,
    Attacking,
    Damaged,
    Dead,
    Max
}

[System.Serializable]
public class State
{
    [SerializeField] CreatureState state = CreatureState.Idle;

    bool isOnJumpableObject = true;
    public bool IsOnJumpableObject { get => isOnJumpableObject; set => isOnJumpableObject = value; }

    public bool CanMove { get => !IsAttacking; }
    public bool CanJump { get => (!IsJumping && !IsAttacking && IsOnJumpableObject && !IsDamaged); }
    public bool CanAttack { get => (!IsJumping && !IsAttacking && !IsDamaged); }
    public bool CanMoveAlongWithMachine { get => !IsDead; }

    public bool IsIdle { get => (state == CreatureState.Idle); }
    public bool IsJumping { get => (state == CreatureState.Jumping); }
    public bool IsAttacking { get => (state == CreatureState.Attacking); }
    public bool IsDamaged { get => (state == CreatureState.Damaged); }
    public bool IsDead { get => (state == CreatureState.Dead); }

    public void InitState()
    {
        SetIdle();
        isOnJumpableObject = true;
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
        isOnJumpableObject = false;
        ChangeState(CreatureState.Jumping);
    }

    public void SetAttacking()
    {
        ChangeState(CreatureState.Attacking);
    }

    public void SetDamaged()
    {
        isOnJumpableObject = false;
        ChangeState(CreatureState.Damaged);
    }

    public void SetDead()
    {
        isOnJumpableObject = false;
        ChangeState(CreatureState.Dead);
    }
}

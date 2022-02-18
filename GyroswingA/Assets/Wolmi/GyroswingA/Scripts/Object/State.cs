using UnityEngine;

public enum CreatureState
{
    Idle,
    Jumping,
    Attacking,
    Dead,
    Max
}

[System.Serializable]
public class State
{
    [SerializeField] CreatureState state = CreatureState.Idle;

    public bool CanMove { get => !IsAttacking; }
    public bool CanJump { get => (!IsJumping && !IsAttacking); }
    public bool CanAttack { get => (!IsJumping && !IsAttacking); }
    public bool CanMoveAlongWithMachine { get => !IsDead; }

    public bool IsIdle { get => (state == CreatureState.Idle); }
    public bool IsJumping { get => (state == CreatureState.Jumping); }
    public bool IsAttacking { get => (state == CreatureState.Attacking); }
    public bool IsDead { get => (state == CreatureState.Dead); }


    public CreatureState GetCurState()
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
        ChangeState(CreatureState.Jumping);
    }

    public void SetAttacking()
    {
        ChangeState(CreatureState.Attacking);
    }

    public void SetDead()
    {
        ChangeState(CreatureState.Dead);
    }
}

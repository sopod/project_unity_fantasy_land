public enum CreatureState
{
    Idle,
    Attacking,
    Dead,
    Max
}

public class State
{
    CreatureState state;

    public bool IsIdle { get { return state == CreatureState.Idle; } }
    public bool IsAttacking { get { return state == CreatureState.Attacking; } }
    public bool IsDead { get { return state == CreatureState.Dead; } }

    public State()
    {
        SetIdle();
    }

    public CreatureState GetCurState()
    {
        return state;
    }

    public void ChangeState(CreatureState inState)
    {
        state = inState;
    }

    public void SetIdle()
    {
        ChangeState(CreatureState.Idle);
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

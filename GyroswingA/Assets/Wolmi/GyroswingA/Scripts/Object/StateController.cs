using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CreatureState
{
    Idle,
    Attacking,
    Dead,
    Max
}

public class StateController
{
    CreatureState State;

    public bool IsIdle { get { return State == CreatureState.Idle; } }
    public bool IsAttacking { get { return State == CreatureState.Attacking; } }
    public bool IsDead { get { return State == CreatureState.Dead; } }

    public StateController()
    {
        SetIdle();
    }

    public CreatureState GetCurState()
    {
        return State;
    }

    public void ChangeState(CreatureState inState)
    {
        State = inState;
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

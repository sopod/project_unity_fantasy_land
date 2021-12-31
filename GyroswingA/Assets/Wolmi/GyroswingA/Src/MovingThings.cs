using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingThings : MonoBehaviour
{
    bool _isPaused;
    bool _isStopped;

    protected void InitMovingThings()
    {
        PauseMoving();
    }

    public void StartMoving()
    {
        _isPaused = false;
        _isStopped = false;
    }

    public void PauseMoving()
    {
        _isPaused = true;
        _isStopped = true;
    }

    public void StopMoving()
    {
        _isPaused = false;
        _isStopped = true;
    }

    public bool IsPaused()
    {
        return _isPaused;
    }

    public bool IsStopped()
    {
        return _isStopped;
    }

}

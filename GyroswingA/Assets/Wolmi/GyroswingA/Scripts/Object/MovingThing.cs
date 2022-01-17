using UnityEngine;

public abstract class MovingThing : MonoBehaviour
{
    bool _isPaused = true;
    bool _isStopped = true;
    

    public virtual void StartMoving()
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

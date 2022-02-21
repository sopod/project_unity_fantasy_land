using UnityEngine;


public abstract class MovingThing : MonoBehaviour
{
    bool _isPaused = true;
    public bool IsPaused { get => _isPaused; }

    bool _isStopped = true;
    public bool IsStopped { get => _isStopped; }

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
}

using UnityEngine;

public abstract class MovingThing : MonoBehaviour
{
    bool _isPaused = true;
    bool _isStopped = true;

    public bool IsPaused { get => _isPaused; }
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

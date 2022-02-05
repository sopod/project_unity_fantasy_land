using UnityEngine;

public class StopWatch
{
    bool timerStarted = false;
    float startTime = 0.0f;
    float limitTime = 0.0f;
    float stopTime = 0.0f;

    public bool IsFinished { get => (timerStarted && GetRemainingTime() <= 0.1f); }
    public bool IsRunning { get => timerStarted; }
    
    public void StartTimer(float limitTime)
    {
        startTime = Time.time;
        this.limitTime = limitTime;
        stopTime = 0.0f;
        timerStarted = true;
    }

    public void PauseTimer()
    {
        if (startTime != 0.0f)
            stopTime = Time.time;
    }

    public void RestartTimer()
    {
        if (stopTime != 0.0f)
        {
            startTime += Time.time - stopTime;
            stopTime = 0.0f;
        }
    }

    public void ExtendTimer(float plusTime)
    {
        limitTime += plusTime;
    }

    public void FinishTimer()
    {
        startTime = 0.0f;
        limitTime = 0.0f;
        timerStarted = false;
    }

    public float GetCurrentTime()
    {
        return Time.time - startTime;
    }
    
    public float GetRemainingTime()
    {
        return limitTime - GetCurrentTime();
    }
}

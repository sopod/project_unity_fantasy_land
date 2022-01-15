using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController
{
    bool timerStarted = false;
    float startTime = 0.0f;
    float limitTime = 0.0f;

    public bool IsFinished 
    {
        get
        {
            return timerStarted && GetRemainingTime() <= 0.0f;
        }
    }

    public bool IsRunning
    {
        get
        {
            return timerStarted;
        }
    }
    
    public void StartTimer(float limitTime)
    {
        startTime = Time.time;
        this.limitTime = limitTime;
        timerStarted = true;
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

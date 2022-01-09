using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController
{
    float startTime = 0.0f;
    float limitTime = 0.0f;

    public bool IsFinished 
    {
        get
        {
            return GetRemainingTime() <= 0.0f && limitTime != 0.0f && startTime != 0.0f;
        }
    }

    public bool IsRunning
    {
        get
        {
            return startTime != 0.0f;
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
    }
    public void StartTimer(float limitTime)
    {
        startTime = Time.time;
        this.limitTime = limitTime;
    }

    public void FinishTimer()
    {
        startTime = 0.0f;
        limitTime = 0.0f;
    }

    public float GetCurrentTime()
    {
        if (startTime == 0.0f) return 0.0f;

        return Time.time - startTime;
    }

    public float GetRemainingTime()
    {
        if (limitTime == 0.0f || startTime == 0.0f) return 0.0f;

        return limitTime - GetCurrentTime();
    }

}

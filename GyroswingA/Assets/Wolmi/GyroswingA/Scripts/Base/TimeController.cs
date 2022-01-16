using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeController
{
    bool timerStarted = false;
    float startTime = 0.0f;
    float limitTime = 0.0f;

    float stopTime = 0.0f;

    public bool IsFinished { get { return timerStarted && GetRemainingTime() <= 0.2f; } }
    public bool IsRunning { get { return timerStarted; } }
    
    public void StartTimer(float limitTime)
    {
        startTime = Time.time;
        this.limitTime = limitTime;
        stopTime = 0.0f;
        timerStarted = true;
    }

    public void PauseTimer()
    {
        stopTime = Time.time;
    }

    public void RestartTimer()
    {
        if (stopTime != 0.0f)
        {
            startTime += Time.time - stopTime;
            stopTime = 0.0f;
        }
        else
        {
            //Debug.Log("Time was not paused before");
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager
{
    float startTime;

    public void StartTimer()
    {
        startTime = Time.time;
    }

    public float GetCurrentTime()
    {
        return Time.time - startTime;
    }

}

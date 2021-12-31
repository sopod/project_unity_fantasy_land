using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
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

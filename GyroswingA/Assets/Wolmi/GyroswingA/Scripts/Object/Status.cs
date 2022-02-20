using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Status
{
    public const float HEALTH_MAX = 100.0f;
    float health = HEALTH_MAX;
    public float HealthSilderValue { get => health / HEALTH_MAX; }
    
    public void InitStatus()
    {
        health = HEALTH_MAX;
    }

    public void ReduceHealth(float damage)
    {
        health -= damage;
        if (health < 0.1f)
        {
            health = 0.0f;
        }
    }

}

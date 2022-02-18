using System;
using UnityEngine;

public class Projectile : MonoBehaviour, ISpawnableObject
{
    public event BackToPoolDelegate BackToPool;
    public void InvokeBackToPool() { BackToPool?.Invoke(); }

    [SerializeField] ProjectileType pjType = ProjectileType.Max;
    public int Type
    {
        get => (int)pjType;
        set
        {
            if (value >= (int)ProjectileType.Max) return;
            pjType = (ProjectileType)value;
        }
    }

    float projectileMoveSpeed = 7.0f;
    float projectileRemaingTime = 1.6f;

    bool isDisabled = false;
    bool isAlive = false;
    public bool isMoving = false;
    
    void OnEnable()
    {
        SetStart();
    }

    void Update()
    {
        if (!isAlive || !isMoving) return;

        MoveForward();
    }

    public void SetStart()
    {
        isDisabled = false;
        isAlive = true;
        
        Invoke("BackToSpawner", projectileRemaingTime);
    }

    void MoveForward()
    {
        transform.Translate(transform.forward * projectileMoveSpeed * Time.deltaTime, Space.World);
    }

    public void BackToSpawner()
    {
        if (isDisabled) return;

        isDisabled = true;
        isAlive = false;

        BackToPool?.Invoke();
    }
    

}

using UnityEngine;


public class Projectile : MonoBehaviour, ISpawnableObject
{
    float projectileMoveSpeed = 7.0f;
    float projectileRemaingTime = 1.6f;
    ProjectileSpawner spawner;

    bool isDisabled = false;
    bool isAlive = false;
    public bool isMoving = false;

    ProjectileType projectileType;

    public int Type
    {
        get => (int)projectileType;
        set { projectileType = (ProjectileType)value; }
    }


    void Update()
    {
        if (!isAlive || !isMoving) return;

        MoveForward();
    }

    void MoveForward()
    {
        transform.Translate(transform.forward * projectileMoveSpeed * Time.deltaTime, Space.World);
    }

    public void SetStart(ProjectileSpawner spawner)
    {
        this.spawner = spawner;

        isDisabled = false;
        isAlive = true;
        
        Invoke("BackToSpawner", projectileRemaingTime);
    }

    public void BackToSpawner()
    {
        if (isDisabled) return;

        isDisabled = true;
        isAlive = false;

        spawner.ReturnObject(this.gameObject, Type);
    }
    

}

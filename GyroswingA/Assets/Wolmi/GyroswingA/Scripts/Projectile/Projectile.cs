using UnityEngine;


public class Projectile : MonoBehaviour, ISpawnableObject
{
    Options options;

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
        transform.Translate(transform.forward * options.ProjectileMoveSpeed * Time.deltaTime, Space.World);
    }

    public void SetStart(Options options)
    {
        this.options = options;

        isDisabled = false;
        isAlive = true;
        
        Invoke("BackToSpawner", options.ProjectileRemaingTime);
    }

    public void BackToSpawner()
    {
        if (isDisabled) return;

        isDisabled = true;
        isAlive = false;

        options.ProjectilesSpawner.ReturnObject(this.gameObject, Type);
    }
    

}

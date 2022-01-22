using UnityEngine;


public class ProjectileController : MonoBehaviour, ISpawnableObject
{
    Options options;
    
    bool isAlive = false;
    public bool isMoving = false;

    ProjectileType projectileType;
    ProjectileSpawner spawner;

    public int Type
    {
        get { return (int)projectileType; }
        set { projectileType = (ProjectileType)value; }
    }


    void Update()
    {
        if (isAlive && isMoving)
        {
            MoveForward();
        }
    }

    void MoveForward()
    {
        transform.Translate(transform.forward * options.ProjectileMoveSpeed * Time.deltaTime, Space.World);
    }

    public void SetStart(ProjectileSpawner spawner, Options options)
    {
        this.spawner = spawner;
        this.options = options;

        isAlive = true;
        
        Invoke("BackToSpawner", options.ProjectileRemaingTime);
    }
    

    public void BackToSpawner()
    {
        CancelInvoke("BackToSpawner");

        isAlive = false;

        spawner.ReturnObject(this.gameObject);
    }
    

}

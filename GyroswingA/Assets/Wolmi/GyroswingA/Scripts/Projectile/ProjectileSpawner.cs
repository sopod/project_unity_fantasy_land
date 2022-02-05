using System.Collections.Generic;
using UnityEngine;


public class ProjectileSpawner : ObjectSpawner
{
    [HideInInspector] public List<Projectile> spawnedProjectiles = new List<Projectile>();

    void Start()
    {
        SetSpawner();
    }

    void SetSpawner()
    {
        spawnedObjectCount = new int[(int)ProjectileType.Max];
        pools = new Queue<GameObject>[(int)ProjectileType.Max];
        
        PrepareObjects((int)ProjectileType.Max, spawnerPrepareAmount);
        InitSpawner();
    }

    public override void InitSpawner()
    {
        ReturnAllObjects();
    }
    
    protected override void SetObjectBeforeSpawned(GameObject e, int idx)
    {
        e.transform.SetParent(null);
        spawnedProjectiles.Add(e.GetComponent<Projectile>());
        spawnedObjectCount[idx]++;
    }
        
    public override void ReturnAllObjects()
    {
        for (int i = spawnedProjectiles.Count - 1; i >= 0; i--)
        {
            ReturnObject(spawnedProjectiles[i].gameObject, spawnedProjectiles[i].Type);
            spawnedProjectiles.RemoveAt(i);
        }
    }
    
    public GameObject SpawnFireProjectile(Vector3 pos, Vector3 forward)
    {
        GameObject p = TakeObject((int)ProjectileType.FireShoot);

        p.transform.position = pos;
        p.transform.forward = forward;

        return p;
    }

    public GameObject SpawnDashHitProjectile(Collision collision)
    {
        GameObject p = TakeObject((int)ProjectileType.DashHit);
        
        p.transform.position = collision.contacts[0].point;
        p.transform.right = collision.contacts[0].normal;

        p.transform.SetParent(collision.gameObject.transform);

        return p;
    }

    public GameObject SpawnFireHitProjectile(GameObject character)
    {
        GameObject p = TakeObject((int)ProjectileType.FireHit);

        p.transform.position = character.transform.position;
        p.transform.forward = character.transform.forward;

        return p;
    }

    public GameObject SpawnDeadProjectile(GameObject character)
    {
        GameObject p = TakeObject((int)ProjectileType.Dead);

        p.transform.position = character.transform.position + character.transform.right * 0.1f + character.transform.up * 0.3f;
        p.transform.forward = character.transform.forward;

        return p;
    }

    public GameObject SpawnItemPickUpProjectile(GameObject character)
    {
        GameObject p = TakeObject((int)ProjectileType.ItemPickUp);

        p.transform.position = character.transform.position;
        p.transform.forward = -character.transform.up;

        p.transform.SetParent(character.transform);

        return p;
    }

}

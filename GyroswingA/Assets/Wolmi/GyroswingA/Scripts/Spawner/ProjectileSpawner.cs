using System.Collections.Generic;
using UnityEngine;


public class ProjectileSpawner : ObjectSpawner
{
    [HideInInspector] public List<Projectile> spawnedProjectiles = new List<Projectile>();

    void Awake()
    {
        InitSpawner((int)ProjectileType.Max);
    }

    public override void ReturnAll()
    {
        ReturnAllObjects<Projectile>(spawnedProjectiles);
    }

    protected override void SetObjectBeforeSpawned(GameObject e, int idx)
    {
        e.transform.SetParent(null);
        spawnedProjectiles.Add(e.GetComponent<Projectile>());
        spawnedObjectCount[idx]++;
    }
    
    public void SpawnFireProjectile(Vector3 pos, Vector3 forward)
    {
        GameObject p = TakeObject((int)ProjectileType.FireShoot);

        p.transform.position = pos;
        p.transform.forward = forward;

        p.SetActive(true);
    }

    public void SpawnDashHitProjectile(Collision collision)
    {
        GameObject p = TakeObject((int)ProjectileType.DashHit);
        
        p.transform.position = collision.contacts[0].point;
        p.transform.right = collision.contacts[0].normal;

        p.transform.SetParent(collision.gameObject.transform);

        p.SetActive(true);
    }

    public void SpawnFireHitProjectile(GameObject character)
    {
        GameObject p = TakeObject((int)ProjectileType.FireHit);

        p.transform.position = character.transform.position;
        p.transform.forward = character.transform.forward;

        p.SetActive(true);
    }

    public void SpawnDeadProjectile(GameObject character)
    {
        GameObject p = TakeObject((int)ProjectileType.Dead);

        p.transform.position = character.transform.position + character.transform.right * 0.1f + character.transform.up * 0.3f;
        p.transform.forward = character.transform.forward;

        p.transform.SetParent(character.transform);
        p.SetActive(true);
    }

    public void SpawnItemPickUpProjectile(GameObject character)
    {
        GameObject p = TakeObject((int)ProjectileType.ItemPickUp);

        p.transform.position = character.transform.position;
        p.transform.forward = -character.transform.up;

        p.transform.SetParent(character.transform);
        
        p.SetActive(true);
    }

}

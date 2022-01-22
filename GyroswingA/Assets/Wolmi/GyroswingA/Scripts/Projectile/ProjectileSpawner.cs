using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    FireShoot,
    FireHit,
    DashHit,
    Dead,
    ItemPickUp,
    Max
}

public class ProjectileSpawner : MonoBehaviour
{
    Options options;

    int[] spawnedObjectCount;

    [SerializeField] ObjectDatabase database;

    Queue<GameObject>[] queues;
    [HideInInspector] public List<GameObject>[] spawnedObjects;

    void Start()
    {
        SetSpawner(new Options());
    }


    public void SetSpawner(Options options)
    {
        this.options = options;

        spawnedObjects = new List<GameObject>[(int) ProjectileType.Max];

        spawnedObjectCount = new int[(int)ProjectileType.Max];

        for (int i = 0; i < spawnedObjects.Length; i++)
        {
            spawnedObjects[i] = new List<GameObject>();
        }

        PrepareObjects(options.SpawnerPrepareAmount);
        InitSpawner();
    }

    public void InitSpawner()
    {
        ReturnAllObjects((int)ProjectileType.FireShoot);
        ReturnAllObjects((int)ProjectileType.DashHit);
    }

    void PrepareObjects(int amount)
    {
        queues = new Queue<GameObject>[(int)ProjectileType.Max];

        for (int i = 0; i < (int)ProjectileType.Max; i++)
        {
            queues[i] = new Queue<GameObject>();
            CreateObject(i, amount);
        }
    }

    void CreateObject(int idx, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject e = Instantiate(database.GetPrefab(idx), Vector3.zero, database.GetPrefab(idx).transform.rotation, this.transform);

            ISpawnableObject obj = e.GetComponent<ISpawnableObject>();
            obj.Type = idx;

            e.SetActive(false);
            queues[idx].Enqueue(e);
        }
    }

    GameObject TakeObject(int idx)
    {
        if (queues[idx].Count == 0)
        {
            CreateObject(idx, 1);
        }

        GameObject e = queues[idx].Dequeue();

        e.transform.SetParent(null);
        
        spawnedObjects[idx].Add(e);
        spawnedObjectCount[idx]++;

        e.SetActive(true);

        return e;
    }

    public void ReturnObject(GameObject e)
    {
        ISpawnableObject obj = e.GetComponent<ISpawnableObject>();
        int idx = obj.Type;

        e.SetActive(false);
        e.transform.SetParent(this.transform);
        queues[idx].Enqueue(e);

        spawnedObjectCount[idx]--;
    }

    public void ReturnAllObjects(int idx)
    {
        for (int i = spawnedObjects[idx].Count - 1; i >= 0; i--)
        {
            ReturnObject(spawnedObjects[idx][i]);
            spawnedObjects[idx].RemoveAt(i);
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

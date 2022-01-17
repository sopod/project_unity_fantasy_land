using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ProjectileType
{
    Shoot,
    Hit, 
    Max
}

public class ProjectileSpawner : MonoBehaviour
{
    Options options;

    //[SerializeField] GameObject stage;
    [SerializeField] int[] spawnedObjectCount;

    [SerializeField] GameObject[] projectiles;

    [SerializeField] Queue<GameObject>[] queues;
    public List<GameObject>[] spawnedObjects;

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
        ReturnAllObjects((int)ProjectileType.Shoot);
        ReturnAllObjects((int)ProjectileType.Hit);
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
            GameObject e = Instantiate(projectiles[idx], Vector3.zero, projectiles[idx].transform.rotation, this.transform);

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

        //e.transform.SetParent(stage.gameObject.transform);
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
    
    
    public GameObject SpawnProjectile(ProjectileType type, Vector3 pos, Vector3 forward)
    {
        GameObject p = TakeObject((int)type);

        p.transform.position = pos;
        p.transform.forward = forward;

        return p;
    }
    



}

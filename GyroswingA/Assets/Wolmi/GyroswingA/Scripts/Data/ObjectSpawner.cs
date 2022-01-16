using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] bool[] isPositionTaken;
    [SerializeField] int[] spawnedObjectCount;

    int totalSpawnedObjectCount { get { return spawnedObjects.Count; } }

    [SerializeField] ObjectDatabase database;
    [SerializeField] GameObject[] spawnPositions;

    [SerializeField] Queue<GameObject>[] queues;
    public List<GameObject> spawnedObjects;
    
    public void SetSpawner(int prepareAamount)
    {
        spawnedObjects = new List<GameObject>();
        isPositionTaken = new bool[spawnPositions.Length];
        spawnedObjectCount = new int[(int)EnemyType.Max];

        PrepareObjects(prepareAamount);
        InitSpawner();
    }

    public void InitSpawner()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            isPositionTaken[i] = false;
        }

        ReturnAllObjects();
    }

    void PrepareObjects(int amount)
    {
        queues = new Queue<GameObject>[(int)EnemyType.Max];

        for (int i = 0; i < (int)EnemyType.Max; i++)
        {
            queues[i] = new Queue<GameObject>();
            CreateObject(i, amount);
        }
    }

    void CreateObject(int idx, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject e = Instantiate(database.GetPrefab(idx), Vector3.zero, Quaternion.identity, this.transform);
            
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

        SetObjectRandomPosition(e);

        spawnedObjects.Add(e);
        spawnedObjectCount[idx]++;

        e.SetActive(true);

        return e;
    }

    void ReturnObject(GameObject e)
    {
        ISpawnableObject obj = e.GetComponent<ISpawnableObject>();
        int idx = obj.Type;

        e.SetActive(false);
        e.transform.SetParent(this.transform);
        queues[idx].Enqueue(e);
        
        spawnedObjectCount[idx]--;
    }

    public void ReturnAllObjects()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            ReturnObject(spawnedObjects[i]);
            spawnedObjects.RemoveAt(i);
        }

        for (int i = 0; i < isPositionTaken.Length; i++)
            isPositionTaken[i] = false;
    }

    void SetObjectRandomPosition(GameObject e)
    {
        int i = Random.Range(0, spawnPositions.Length);
        
        while (isPositionTaken[i])
        {
            i = Random.Range(0, spawnPositions.Length);
        }

        if (isPositionTaken[i])
        {
            Debug.LogWarning("Object Count Over Position Index");
            return;
        }

        e.transform.position = spawnPositions[i].transform.position;
        isPositionTaken[i] = true;
    }

    public GameObject SpawnEnemyObject(EnemyType[] types, int maxAmount)
    {
        if (totalSpawnedObjectCount >= maxAmount)
        {
            Debug.Log("Already spawned all");
            return null;
        }

        // at least one object have to gen
        for (int i = 0; i < types.Length; i++)
        {
            if (spawnedObjectCount[(int)types[i]] == 0)
            {
                return TakeObject((int)types[i]);
            }
        }

        int idx = Random.Range(0, types.Length);

        return TakeObject((int)types[idx]);
    }

    public GameObject SpawnItemObject(ItemType type)
    {
        return TakeObject((int)type);
    }
}
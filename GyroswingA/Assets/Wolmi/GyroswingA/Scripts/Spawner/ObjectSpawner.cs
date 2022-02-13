using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectSpawner : MonoBehaviour
{
    protected const int PREPARE_MAX = 10;

    protected bool[] isPositionTaken;
    protected int[] spawnedObjectCount;

    [SerializeField] protected ObjectDatabase database;
    [SerializeField] protected GameObject[] spawnPositions;

    protected Queue<GameObject>[] pools;



    protected abstract void InitSpawner();

    protected void PrepareObjects(int typeMax, int amount)
    {
        for (int i = 0; i < typeMax; i++)
        {
            pools[i] = new Queue<GameObject>();
            CreateObject(i, amount);
        }
    }

    protected void CreateObject(int idx, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject o = Instantiate(database.GetPrefab(idx), Vector3.zero, database.GetPrefab(idx).transform.rotation, transform);
            
            o.GetComponent<ISpawnableObject>().Type = idx;

            o.SetActive(false);
            pools[idx].Enqueue(o);
        }
    }

    protected GameObject TakeObject(int idx)
    {
        if (pools[idx].Count == 0)
        {
            CreateObject(idx, 1);
        }

        GameObject o = pools[idx].Dequeue();

        SetObjectBeforeSpawned(o, idx);

        o.SetActive(true);

        return o;
    }

    protected abstract void SetObjectBeforeSpawned(GameObject e, int idx);

    public void ReturnObject(GameObject e, int idx)
    {
        e.SetActive(false);
        e.transform.SetParent(this.transform);
        pools[idx].Enqueue(e);
        
        spawnedObjectCount[idx]--;
    }

    public abstract void ReturnAllObjects();

    protected void SetObjectRandomPosition(GameObject obj, int idx)
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
        
        obj.transform.position = spawnPositions[i].transform.position + new Vector3(0, database.GetPrefab(idx).transform.position.y, 0);

        isPositionTaken[i] = true;
    }
    
}

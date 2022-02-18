using System.Collections.Generic;
using UnityEngine;


// Object Pool을 사용하였습니다. 


public abstract class ObjectSpawner : MonoBehaviour
{
    protected const int OBJECT_PREPARE_AMOUNT = 10;

    [SerializeField] ObjectDatabase database;
    protected Queue<GameObject>[] pools;
    [SerializeField] protected GameObject[] spawnPositions;

    protected bool[] isPositionTaken;
    protected int[] spawnedObjectCount;

    protected void InitSpawner(int max)
    {
        isPositionTaken = new bool[spawnPositions.Length];
        spawnedObjectCount = new int[max];
        pools = new Queue<GameObject>[max];

        PrepareObjects(max, OBJECT_PREPARE_AMOUNT);

        for (int i = 0; i < spawnPositions.Length; i++)
            isPositionTaken[i] = false;
    }

    void PrepareObjects(int typeMax, int amount)
    {
        for (int i = 0; i < typeMax; i++)
        {
            pools[i] = new Queue<GameObject>();
            CreateObject(i, amount);
        }
    }

    void CreateObject(int idx, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject o = Instantiate(database.GetPrefab(idx), Vector3.zero, database.GetPrefab(idx).transform.rotation, transform);
            ISpawnableObject obj = o.GetComponent<ISpawnableObject>();

            obj.BackToPool += () =>
            {
                ReturnObject(o, idx);
            };

            obj.Type = idx;
            
            o.SetActive(false);
            pools[idx].Enqueue(o);
        }
    }

    protected GameObject TakeObject(int idx)
    {
        if (pools[idx].Count == 0)
            CreateObject(idx, 1);

        GameObject o = pools[idx].Dequeue();
        SetObjectBeforeSpawned(o, idx);

        return o;
    }

    protected abstract void SetObjectBeforeSpawned(GameObject e, int idx);

    void ReturnObject(GameObject o, int idx)
    {
        o.SetActive(false);
        o.transform.SetParent(this.transform);
        pools[idx].Enqueue(o);
        spawnedObjectCount[idx]--;
    }

    protected void ReturnAllObjects<T>(List<T> obj) where T : ISpawnableObject
    {
        for (int i = obj.Count - 1; i >= 0; i--)
        {
            obj[i].InvokeBackToPool();
        }

        obj.Clear();

        for (int i = 0; i < isPositionTaken.Length; i++)
            isPositionTaken[i] = false;
    }

    public abstract void ReturnAll();

    protected void SetObjectRandomPosition(GameObject o, int idx)
    {
        int i = Random.Range(0, spawnPositions.Length);

        while (isPositionTaken[i])
            i = Random.Range(0, spawnPositions.Length);

        if (isPositionTaken[i])
        {
            Debug.LogWarning("스폰할 위치가 부족합니다.");
            return;
        }
        
        o.transform.position = spawnPositions[i].transform.position + new Vector3(0, database.GetPrefab(idx).transform.position.y, 0);
        isPositionTaken[i] = true;
    }
    
}

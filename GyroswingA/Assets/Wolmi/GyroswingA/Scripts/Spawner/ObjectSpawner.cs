using System.Collections.Generic;
using UnityEngine;


// Object Pool을 사용하였습니다. 


public abstract class ObjectSpawner : MonoBehaviour
{
    protected const int OBJECT_PREPARE_AMOUNT = 10;

    protected bool[] isPositionTaken;
    protected int[] spawnedObjectCount;

    [SerializeField] ObjectDatabase database;
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

    void CreateObject(int idx, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject o = Instantiate(database.GetPrefab(idx), Vector3.zero, database.GetPrefab(idx).transform.rotation, transform);
            o.GetComponent<ISpawnableObject>().Type = idx;
            o.SetActive(false);
            pools[idx].Enqueue(o);
        }
    }

    // Object Pool에서 index에 해당하는 오브젝트를 꺼냅니다. 
    protected GameObject TakeObject(int idx)
    {
        if (pools[idx].Count == 0)
            CreateObject(idx, 1);

        GameObject o = pools[idx].Dequeue();
        SetObjectBeforeSpawned(o, idx);
        o.SetActive(true);

        return o;
    }

    protected abstract void SetObjectBeforeSpawned(GameObject e, int idx);

    // 다쓴 오브젝트를 다시 Object Pool로 되돌려 보냅니다. 
    public void ReturnObject(GameObject o, int idx)
    {
        o.SetActive(false);
        o.transform.SetParent(this.transform);
        pools[idx].Enqueue(o);
        spawnedObjectCount[idx]--;
    }

    public abstract void ReturnAllObjects();

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

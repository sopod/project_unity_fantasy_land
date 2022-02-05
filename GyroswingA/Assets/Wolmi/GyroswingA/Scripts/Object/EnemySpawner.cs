using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : ObjectSpawner
{
    [HideInInspector] public List<Enemy> spawnedEnemies = new List<Enemy>();

    void Start()
    {
        SetSpawner();
    }

    void SetSpawner()
    {
        isPositionTaken = new bool[spawnPositions.Length];
        spawnedObjectCount = new int[(int)EnemyType.Max];
        pools = new Queue<GameObject>[(int)EnemyType.Max];

        PrepareObjects((int)EnemyType.Max, spawnerPrepareAmount);
        InitSpawner();
    }

    protected override void SetObjectBeforeSpawned(GameObject o, int idx)
    {
        o.transform.SetParent(null);

        SetObjectRandomPosition(o, idx);

        spawnedEnemies.Add(o.GetComponent<Enemy>());
        spawnedObjectCount[idx]++;
    }

    public override void ReturnAllObjects()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            ReturnObject(spawnedEnemies[i].gameObject, spawnedEnemies[i].Type);
            spawnedEnemies.RemoveAt(i);
        }

        for (int i = 0; i < isPositionTaken.Length; i++)
            isPositionTaken[i] = false;
    }

    public GameObject SpawnEnemyObject(EnemyType[] types, int maxAmount)
    {
        if (spawnedEnemies.Count >= maxAmount)
        {
            Debug.LogWarning("Already spawned all");
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
}

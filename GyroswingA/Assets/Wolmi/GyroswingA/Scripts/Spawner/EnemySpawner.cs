using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : ObjectSpawner
{
    [HideInInspector] public List<Enemy> spawnedEnemies = new List<Enemy>();

    void Awake()
    {
        InitSpawner((int)EnemyType.Max);
    }

    public override void ReturnAll()
    {
        ReturnAllObjects<Enemy>(spawnedEnemies);
    }

    protected override void SetObjectBeforeSpawned(GameObject o, int idx)
    {
        o.transform.SetParent(null);

        SetObjectRandomPosition(o, idx);

        spawnedEnemies.Add(o.GetComponent<Enemy>());
        spawnedObjectCount[idx]++;
    }

    public GameObject SpawnEnemyObject(EnemyType[] types, int maxAmount)
    {
        if (spawnedEnemies.Count >= maxAmount)
        {
            Debug.LogWarning("이미 모든 몬스터가 스폰되었습니다.");
            return null;
        }

        GameObject o;

        for (int i = 0; i < types.Length; i++)
        {
            if (spawnedObjectCount[(int)types[i]] == 0)
            {
                o = TakeObject((int)types[i]);
                o.SetActive(true);
                return o;
            }
        }

        int idx = Random.Range(0, types.Length);

        o = TakeObject((int)types[idx]);
        o.SetActive(true);
        return o;
    }
}

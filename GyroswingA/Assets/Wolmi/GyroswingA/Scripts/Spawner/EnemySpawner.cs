using System.Collections.Generic;
using UnityEngine;


// 적 몬스터의 Object Pool 클래스입니다. 


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

    // 스폰하기 전에 부모 설정과 위치를 세팅합니다. 
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

        // 최소 1마리의 몬스터가 종류별로 스폰되어야 합니다. 
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

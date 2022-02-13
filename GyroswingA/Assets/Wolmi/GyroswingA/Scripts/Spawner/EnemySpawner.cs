using System.Collections.Generic;
using UnityEngine;


// 적 몬스터의 Object Pool 클래스입니다. 


public class EnemySpawner : ObjectSpawner
{
    [HideInInspector] public List<Enemy> spawnedEnemies = new List<Enemy>();

    void Awake()
    {
        InitSpawner();
    }

    protected override void InitSpawner()
    {
        isPositionTaken = new bool[spawnPositions.Length];
        spawnedObjectCount = new int[(int)EnemyType.Max];
        pools = new Queue<GameObject>[(int)EnemyType.Max];

        PrepareObjects((int)EnemyType.Max, OBJECT_PREPARE_AMOUNT);

        for (int i = 0; i < spawnPositions.Length; i++)
            isPositionTaken[i] = false; 

        ReturnAllObjects();
    }

    // 스폰하기 전에 부모 설정과 위치를 세팅합니다. 
    protected override void SetObjectBeforeSpawned(GameObject o, int idx)
    {
        o.transform.SetParent(null);

        SetObjectRandomPosition(o, idx);

        spawnedEnemies.Add(o.GetComponent<Enemy>());
        spawnedObjectCount[idx]++;
    }

    // 스폰되어 있는 모든 적을 Object Pool로 반환합니다. 
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

    // 몬스터를 스폰합니다. 
    public GameObject SpawnEnemyObject(EnemyType[] types, int maxAmount)
    {
        if (spawnedEnemies.Count >= maxAmount)
        {
            Debug.LogWarning("이미 모든 몬스터가 스폰되었습니다.");
            return null;
        }

        // 최소 1마리의 몬스터가 종류별로 스폰되어야 합니다. 
        for (int i = 0; i < types.Length; i++)
        {
            if (spawnedObjectCount[(int)types[i]] == 0)
                return TakeObject((int)types[i]);
        }

        int idx = Random.Range(0, types.Length);

        return TakeObject((int)types[idx]);
    }
}

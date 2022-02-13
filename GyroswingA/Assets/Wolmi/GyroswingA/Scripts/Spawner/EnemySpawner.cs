using System.Collections.Generic;
using UnityEngine;


// �� ������ Object Pool Ŭ�����Դϴ�. 


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

    // �����ϱ� ���� �θ� ������ ��ġ�� �����մϴ�. 
    protected override void SetObjectBeforeSpawned(GameObject o, int idx)
    {
        o.transform.SetParent(null);

        SetObjectRandomPosition(o, idx);

        spawnedEnemies.Add(o.GetComponent<Enemy>());
        spawnedObjectCount[idx]++;
    }

    // �����Ǿ� �ִ� ��� ���� Object Pool�� ��ȯ�մϴ�. 
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

    // ���͸� �����մϴ�. 
    public GameObject SpawnEnemyObject(EnemyType[] types, int maxAmount)
    {
        if (spawnedEnemies.Count >= maxAmount)
        {
            Debug.LogWarning("�̹� ��� ���Ͱ� �����Ǿ����ϴ�.");
            return null;
        }

        // �ּ� 1������ ���Ͱ� �������� �����Ǿ�� �մϴ�. 
        for (int i = 0; i < types.Length; i++)
        {
            if (spawnedObjectCount[(int)types[i]] == 0)
                return TakeObject((int)types[i]);
        }

        int idx = Random.Range(0, types.Length);

        return TakeObject((int)types[idx]);
    }
}

using System.Collections.Generic;
using UnityEngine;


// �� ������ Object Pool Ŭ�����Դϴ�. 


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

    // �����ϱ� ���� �θ� ������ ��ġ�� �����մϴ�. 
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
            Debug.LogWarning("�̹� ��� ���Ͱ� �����Ǿ����ϴ�.");
            return null;
        }

        GameObject o;

        // �ּ� 1������ ���Ͱ� �������� �����Ǿ�� �մϴ�. 
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

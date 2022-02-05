using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : ObjectSpawner
{
    [SerializeField] protected GameObject stage;
    [HideInInspector] public List<Item> spawnedItems = new List<Item>();

    void Start()
    {
        SetSpawner();
    }

    void SetSpawner()
    {
        isPositionTaken = new bool[spawnPositions.Length];
        spawnedObjectCount = new int[(int)ItemType.Max];
        pools = new Queue<GameObject>[(int)ItemType.Max];

        PrepareObjects((int)ItemType.Max, spawnerPrepareAmount);
        InitSpawner();
    }

    protected override void SetObjectBeforeSpawned(GameObject o, int idx)
    {
        o.transform.SetParent(stage.gameObject.transform);

        SetObjectRandomPosition(o, idx);

        spawnedItems.Add(o.GetComponent<Item>());
        spawnedObjectCount[idx]++;

    }

    public override void ReturnAllObjects()
    {
        for (int i = spawnedItems.Count - 1; i >= 0; i--)
        {
            ReturnObject(spawnedItems[i].gameObject, spawnedItems[i].Type);
            spawnedItems.RemoveAt(i);
        }

        for (int i = 0; i < isPositionTaken.Length; i++)
            isPositionTaken[i] = false;
    }

    public GameObject SpawnItemObject(ItemType type)
    {
        return TakeObject((int)type);
    }
}

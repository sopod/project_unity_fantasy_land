using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : ObjectSpawner
{
    [SerializeField] protected GameObject stage;
    [HideInInspector] public List<Item> spawnedItems = new List<Item>();

    void Awake()
    {
        InitSpawner((int)ItemType.Max);
    }

    public override void ReturnAll()
    {
        ReturnAllObjects<Item>(spawnedItems);
    }

    protected override void SetObjectBeforeSpawned(GameObject o, int idx)
    {
        o.transform.SetParent(stage.gameObject.transform);

        SetObjectRandomPosition(o, idx);

        spawnedItems.Add(o.GetComponent<Item>());
        spawnedObjectCount[idx]++;

    }
    
    public GameObject SpawnItemObject(ItemType type)
    {
        GameObject o = TakeObject((int)type);
        o.SetActive(true);
        return o;
    }
}

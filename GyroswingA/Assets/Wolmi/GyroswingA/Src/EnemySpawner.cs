using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    int yongEnemyMax { get { return 4; } }

    bool[] isPositionTaken;
    int[] spawnedEnemyCount;

    GameObject[] enemyPrefabs;
    GameObject[] enemySpawnPositions;

    Queue<GameObject>[] enemyQueues;
    [HideInInspector] public List<GameObject> spawnedEnemies;

    public void SetEnemySpawner(GameObject[] enemyPrefabs, GameObject[] enemySpawnPositions, int prepareAamount)
    {
        this.enemyPrefabs = enemyPrefabs;
        this.enemySpawnPositions = enemySpawnPositions;

        spawnedEnemies = new List<GameObject>();
        isPositionTaken = new bool[enemySpawnPositions.Length];
        spawnedEnemyCount = new int[enemyPrefabs.Length];

        PrepareAllEnemies(prepareAamount);
        InitEnemySpawner();
    }

    public void InitEnemySpawner()
    {
        for (int i = 0; i < enemySpawnPositions.Length; i++)
        {
            isPositionTaken[i] = false;
        }

        ReturnAllEnemy();
    }

    void PrepareAllEnemies(int amount)
    {
        enemyQueues = new Queue<GameObject>[enemyPrefabs.Length];

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemyQueues[i] = new Queue<GameObject>();
            MakeEnemy(i, amount);
        }
    }

    void MakeEnemy(int idx, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject e = Instantiate(enemyPrefabs[idx], Vector3.zero, Quaternion.identity, this.transform);
            e.SetActive(false);
            enemyQueues[idx].Enqueue(e);
        }
    }

    GameObject TakeEnemy(int idx)
    {
        if (enemyQueues[idx].Count == 0)
        {
            MakeEnemy(idx, 1);
        }

        GameObject e = enemyQueues[idx].Dequeue();
        e.transform.SetParent(null); 

        SetEnemyPosition(e);
        
        spawnedEnemies.Add(e);

        e.SetActive(true);

        return e;
    }

    void ReturnEnemy(GameObject e)
    {
        int getThisFromEnemyScript = 0;

        e.SetActive(false);
        e.transform.SetParent(this.transform);
        enemyQueues[getThisFromEnemyScript].Enqueue(e);

        spawnedEnemies.Remove(e);
        spawnedEnemyCount[getThisFromEnemyScript]--;
    }

    void ReturnAllEnemy()
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            ReturnEnemy(spawnedEnemies[i]);
        }
    }

    void SetEnemyPosition(GameObject e)
    {
        int i = Random.Range(0, enemySpawnPositions.Length);

        while (isPositionTaken[i] == true)
        {
            i = Random.Range(0, enemySpawnPositions.Length);
        }

        if (isPositionTaken[i] == true)
        {
            Debug.LogWarning("Enemy Count Over Position Index");
            return;
        }

        e.transform.position = enemySpawnPositions[i].transform.position;
        e.GetComponent<EnemyController>().orgPos = enemySpawnPositions[i].transform.position;
        isPositionTaken[i] = true;
    }

    public GameObject SpawnRandomEnemy()
    {
        // consider spawnedEnemyCount..

        //return TakeEnemy(Random.Range(0, enemyPrefabs.Length));
        return TakeEnemy(0);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTest : MonoBehaviour
{
    [SerializeField] GameObject enemy;

    void Start()
    {
        Instantiate(enemy, new Vector3(-31, 3, -50), Quaternion.identity, null);
    }
    
}

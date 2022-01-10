using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Bwun,
    Ssak,
    Juck,
    Swook,
    Ral,
    Gum,
    Max
}


[CreateAssetMenu(fileName = "Enemy Database", menuName = "Gyroswing/Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    [SerializeField] GameObject[] enemyPrefabs;

    public int YongEnemyAmount { get { return 4; } }

    public GameObject GetEnemy(EnemyType type)
    {
        return enemyPrefabs[(int) type];
    }
}

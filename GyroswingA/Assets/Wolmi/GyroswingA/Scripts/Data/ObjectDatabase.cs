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

public enum ItemType
{
    HarippoBlue,
    HarippoGreen,
    HarippoYellow,
    HarippoRed,
    Coke,
    ChocoTarte,
    Max
}


[CreateAssetMenu(fileName = "Object Database", menuName = "Gyroswing/Object Database")]
public class ObjectDatabase : ScriptableObject
{
     [SerializeField] protected GameObject[] prefabs;

     public GameObject GetPrefab(int type)
     {
         return prefabs[type];
     }
}

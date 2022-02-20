


using UnityEngine;

public class BlackBoard
{
    const float playerDetectionRadius = 2.0f;
    const float playerAttackRadius = 0.5f;
    public float PlayerDetectionRadius { get => playerDetectionRadius; }
    public float PlayerAttackRadius { get => playerAttackRadius; }

    public Enemy OwnerCharacter;
    public Layers Layers;
    public Transform player;
    ObjectValues values;

    public float EnemyWaitTime { get => values.EnemyWaitTime; }
    public float EnemyTurnTime { get => values.EnemyTurnTime; }
    public float EnemyMoveTime { get => values.EnemyMoveTime; }
    public float EnemyLongTurnTime { get => values.EnemyLongTurnTime; }

    public BlackBoard(Enemy ownerCharacter, Layers layers, ObjectValues values, Transform player)
    {
        this.OwnerCharacter = ownerCharacter;
        this.Layers = layers;
        this.values = values;
        this.player = player;
    }
}

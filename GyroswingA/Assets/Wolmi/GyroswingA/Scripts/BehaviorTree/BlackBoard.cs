


public class BlackBoard
{
    public Enemy OwnerCharacter;
    public Layers Layers;
    ObjectValues values;

    // detection range
    const float playerDetectionRadius = 2.0f;
    const float playerAttackRadius = 0.5f;

    public float PlayerDetectionRadius { get => playerDetectionRadius; }
    public float PlayerAttackRadius { get => playerAttackRadius; }

    // time duration
    public float EnemyWaitTime { get => values.EnemyWaitTime; }
    public float EnemyTurnTime { get => values.EnemyTurnTime; }
    public float EnemyMoveTime { get => values.EnemyMoveTime; }
    public float EnemyLongTurnTime { get => values.EnemyLongTurnTime; }

    public BlackBoard(Enemy ownerCharacter, Layers layers, ObjectValues values)
    {
        this.OwnerCharacter = ownerCharacter;
        this.Layers = layers;
        this.values = values;
    }
}

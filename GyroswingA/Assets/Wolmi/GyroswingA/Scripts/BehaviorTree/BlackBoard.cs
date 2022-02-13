

public class BlackBoard
{
    public Enemy character;
    public Layers layerStruct;

    // detection range
    public float playerDetectionRadius { get => 2.0f; }
    public float playerAttackRadius { get => 0.5f; }

    // time duration
    public float enemyWaitTime { get => 1.0f; }
    public float enemyTurnTime { get => 0.3f; }
    public float enemyMoveTime { get => 0.5f; }
    public float enemyLongTurnTime { get => 0.5f; }

    public BlackBoard(Enemy e, Layers layer)
    {
        character = e;
        layerStruct = layer;
    }
}

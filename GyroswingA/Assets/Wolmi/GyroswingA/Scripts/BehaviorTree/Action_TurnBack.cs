

public class Action_TurnBack : Node
{
    public Action_TurnBack(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        if (!addedToMovementQueue)
        {
            addedToMovementQueue = true;
            bb.character.GetComponent<Enemy>().AddEnemyMovement(new MovementData(EnemyMovement.TurnRight, this, bb.enemyLongTurnTime));
        }

        CheckFinishFlag();
        return state;
    }

}
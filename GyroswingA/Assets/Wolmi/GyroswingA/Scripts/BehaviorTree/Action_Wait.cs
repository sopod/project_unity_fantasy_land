public class Action_Wait : Node
{
    public Action_Wait(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        if (!addedToMovementQueue)
        {
            addedToMovementQueue = true;
            bb.character.GetComponent<EnemyController>().AddEnemyMovement(new MovementData(EnemyMovement.Wait, this, bb.enemyWaitTime));
        }

        CheckFinishFlag();

        return state;
    }
}

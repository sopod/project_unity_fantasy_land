


public class Action_Wait : Node
{
    public Action_Wait(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        // EnemyWaitTime 시간 동안 움직이지 않고 기다립니다. 
        if (!addedToMovementQueue)
        {
            addedToMovementQueue = true;
            bb.OwnerCharacter.AddEnemyMovement(new MovementData(EnemyMovement.Wait, this, bb.EnemyWaitTime));
        }

        CheckFinishFlag();

        return state;
    }
}

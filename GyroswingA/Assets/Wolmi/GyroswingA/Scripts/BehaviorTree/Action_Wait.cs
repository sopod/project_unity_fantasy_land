


public class Action_Wait : Node
{
    public Action_Wait(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        // EnemyWaitTime �ð� ���� �������� �ʰ� ��ٸ��ϴ�. 
        if (!addedToMovementQueue)
        {
            addedToMovementQueue = true;
            bb.OwnerCharacter.AddEnemyMovement(new MovementData(EnemyMovement.Wait, this, bb.EnemyWaitTime));
        }

        CheckFinishFlag();

        return state;
    }
}

using UnityEngine;


public class Action_TurnBack : Node
{
    public Action_TurnBack(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        // ������ Ȯ���� �� Ȥ�� ��� ���� �ڸ� �ٶ󺾴ϴ�. 
        if (!addedToMovementQueue)
        {
            addedToMovementQueue = true;
            int ranNum = Random.Range(0, 2);
            EnemyMovement move = (ranNum == 0) ? EnemyMovement.TurnLeft : EnemyMovement.TurnRight;

            bb.OwnerCharacter.AddEnemyMovement(new MovementData(move, this, bb.EnemyLongTurnTime));
        }

        CheckFinishFlag();
        return state;
    }

}
using UnityEngine;


public class Action_TurnBack : Node
{
    public Action_TurnBack(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        // 절반의 확률로 좌 혹은 우로 돌아 뒤를 바라봅니다. 
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
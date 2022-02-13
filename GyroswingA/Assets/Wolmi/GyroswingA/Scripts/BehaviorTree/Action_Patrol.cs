using UnityEngine;


public class Action_Patrol : Node
{
    public Action_Patrol(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        // 절반의 확률로 좌 혹은 후를 바라본 후 앞으로 전진합니다. 
        if (!addedToMovementQueue)
        {
            addedToMovementQueue = true;
            int ranNum = Random.Range(0, 2);
            EnemyMovement move = (ranNum == 0) ? EnemyMovement.TurnLeft : EnemyMovement.TurnRight;

            bb.OwnerCharacter.AddEnemyMovement(new MovementData(move, null, bb.EnemyTurnTime));
            bb.OwnerCharacter.AddEnemyMovement(new MovementData(EnemyMovement.MoveForward, this, bb.EnemyMoveTime));
        }

        CheckFinishFlag();
        return state;
    }
}

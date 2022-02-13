using UnityEngine;


public class Action_Patrol : Node
{
    public Action_Patrol(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        if (!addedToMovementQueue)
        {
            addedToMovementQueue = true;

            int ranNum = Random.Range(0, 2);

            if (ranNum == 0)
            {
                bb.character.AddEnemyMovement(new MovementData(EnemyMovement.TurnLeft, null, bb.enemyTurnTime));
            }
            else if (ranNum == 1)
            {
                bb.character.AddEnemyMovement(new MovementData(EnemyMovement.TurnLeft, null, bb.enemyTurnTime));
            }

            bb.character.AddEnemyMovement(new MovementData(EnemyMovement.MoveForward, this, bb.enemyMoveTime));
        }

        CheckFinishFlag();
        return state;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Action_Patrol : Node
{
    public Action_Patrol(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        if (!addedToMovementQueue)
        {
            addedToMovementQueue = true;

            int ranNum = Random.RandomRange(0, 2);

            if (ranNum == 0)
            {
                bb.character.GetComponent<EnemyController>().AddEnemyMovement(new MovementData(EnemyMovement.TurnLeft, this, bb.enemyTurnTime));
                bb.character.GetComponent<EnemyController>().AddEnemyMovement(new MovementData(EnemyMovement.Wait, this, bb.enemyWaitTime));
            }
            else if (ranNum == 1)
            {
                bb.character.GetComponent<EnemyController>().AddEnemyMovement(new MovementData(EnemyMovement.TurnLeft, this, bb.enemyTurnTime));
                bb.character.GetComponent<EnemyController>().AddEnemyMovement(new MovementData(EnemyMovement.Wait, this, bb.enemyWaitTime));
            }

            bb.character.GetComponent<EnemyController>().AddEnemyMovement(new MovementData(EnemyMovement.MoveForward, this, bb.enemyMoveTime));
        }

        CheckFinishFlag();
        return state;
    }

}

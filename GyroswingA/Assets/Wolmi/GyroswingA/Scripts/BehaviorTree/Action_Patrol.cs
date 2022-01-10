using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Action_Patrol : Node
{
    public Action_Patrol(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        //return BT_State.Success;

        bb.character.GetComponent<EnemyController>().AddEnemyMovement(EnemyMovement.TurnLeft, this);
        bb.character.GetComponent<EnemyController>().AddEnemyMovement(EnemyMovement.MoveForward, this);
        bb.character.GetComponent<EnemyController>().AddEnemyMovement(EnemyMovement.TurnRight, this);
        bb.character.GetComponent<EnemyController>().AddEnemyMovement(EnemyMovement.MoveForward, this);

        CheckFinishFlag();
        return state;
    }

}

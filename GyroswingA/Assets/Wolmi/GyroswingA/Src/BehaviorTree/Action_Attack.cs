using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Attack : Node
{
    public Action_Attack(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        bb.character.GetComponent<EnemyController>().AttackPlayer();

        state = BT_State.Success;

        return state;
    }
}

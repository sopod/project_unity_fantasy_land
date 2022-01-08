using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Attack : Node
{
    public Action_Attack(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        if (true)
            return BT_State.Failure;
        else
            return BT_State.Success;
    }
}

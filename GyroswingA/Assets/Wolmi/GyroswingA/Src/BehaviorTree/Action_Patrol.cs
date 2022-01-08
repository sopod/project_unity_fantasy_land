using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Action_Patrol : Node
{
    public Action_Patrol(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        return BT_State.Failure;
    }

}

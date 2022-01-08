using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector(BlackBoard bb) : base(bb) { }
    public Selector(BlackBoard bb, List<Node> nodes) : base(bb, nodes) { }

    public override BT_State Execute()
    {
        // stop doing if it succeeded
        for (int i = 0; i < children.Count; i++)
        {
            switch (children[i].Execute())
            {
                case BT_State.Running:
                {
                    state = BT_State.Running;
                    return state;
                }

                case BT_State.Success:
                {
                    state = BT_State.Success;
                    return state;
                }

                case BT_State.Failure:
                {
                    continue;
                }

                default:
                {
                    continue;
                }
            }
        }

        // has no children
        state = BT_State.Failure;
        return state;
    }
}
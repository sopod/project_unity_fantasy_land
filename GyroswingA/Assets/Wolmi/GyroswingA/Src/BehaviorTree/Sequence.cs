using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(BlackBoard bb) : base(bb) { }
    public Sequence(BlackBoard bb, List<Node> nodes) : base(bb, nodes) { }

    public override BT_State Execute()
    {
        // stop doing if it failed
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
                    continue;
                }

                case BT_State.Failure:
                {
                    state = BT_State.Failure;
                    return state;
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

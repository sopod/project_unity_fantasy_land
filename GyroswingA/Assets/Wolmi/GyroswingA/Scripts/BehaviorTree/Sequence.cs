using System.Collections.Generic;


public class Sequence : Node
{
    public Sequence(BlackBoard bb) : base(bb) { }
    public Sequence(BlackBoard bb, List<Node> nodes) : base(bb, nodes) { }

    public override BT_State Execute()
    {
        bool anyChildIsRunning = false;

        for (int i = 0; i < children.Count; i++)
        {
            switch (children[i].Execute())
            {
                case BT_State.Running:
                {
                    anyChildIsRunning = true;
                    continue;
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
            }
        }

        state = (anyChildIsRunning) ? BT_State.Running : BT_State.Success;
        return state;
    }
}

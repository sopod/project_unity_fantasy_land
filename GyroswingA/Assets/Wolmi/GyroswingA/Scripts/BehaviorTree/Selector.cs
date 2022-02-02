using System.Collections.Generic;


public class Selector : Node
{
    public Selector(BlackBoard bb) : base(bb) { }
    public Selector(BlackBoard bb, List<Node> nodes) : base(bb, nodes) { }

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
                    state = BT_State.Success;
                    return state;
                }

                case BT_State.Failure:
                {
                    continue;
                }

                default:
                {
                    state = BT_State.Success;
                    return state;
                }
            }
        }

        state = (anyChildIsRunning) ? BT_State.Running : BT_State.Failure;
        return state;
    }
}
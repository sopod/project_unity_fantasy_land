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
                // 자식 노드가 실행중이라면, anyChildIsRunning을 true로 바꿉니다.
                case BT_State.Running:
                {
                    anyChildIsRunning = true;
                    continue;
                }
                // 자식 노드가 성공상태라면, 다음 자식을 봅니다. 
                case BT_State.Success:
                {
                    continue;
                }
                // 자식 노드가 실패상태라면, 실패상태를 반환합니다.
                case BT_State.Failure:
                {
                    state = BT_State.Failure;
                    return state;
                }
            }
        }

        // 자식 노드가 실행중이라면 실행중 상태를 반환하고, 아니면 모두 성공 했으므로, 성공 상태를 반환합니다. 
        state = (anyChildIsRunning) ? BT_State.Running : BT_State.Success;
        return state;
    }

}

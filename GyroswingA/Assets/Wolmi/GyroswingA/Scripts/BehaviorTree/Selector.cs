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
                // �ڽ� ��尡 �������̶��, anyChildIsRunning�� true�� �ٲߴϴ�. 
                case BT_State.Running:
                {
                    anyChildIsRunning = true;
                    continue;
                }
                // �ڽ� ��尡 �������¶��, �������¸� ��ȯ�մϴ�.
                case BT_State.Success:
                {
                    state = BT_State.Success;
                    return state;
                }
                // �ڽ� ��尡 ���л��¶��, ���� �ڽ��� ���ϴ�. 
                case BT_State.Failure:
                {
                    continue;
                }
            }
        }

        // �ڽ� ��尡 �������̶�� ������ ���¸� ��ȯ�ϰ�, �ƴϸ� ��� ���������Ƿ�, ���� ���¸� ��ȯ�մϴ�. 
        state = (anyChildIsRunning) ? BT_State.Running : BT_State.Failure;
        return state;
    }
}
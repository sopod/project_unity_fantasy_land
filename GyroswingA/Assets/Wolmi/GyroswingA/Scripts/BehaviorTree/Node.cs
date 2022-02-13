using System.Collections.Generic;


public enum BT_State
{
    Running,
    Success,
    Failure
}

// �ൿ Ʈ���� ����Դϴ�. ���� ũ�� Selector, Sequence, Action���� �ֽ��ϴ�. 
public abstract class Node
{
    protected BlackBoard bb;
    protected BT_State state;
    protected Node parent = null;
    protected List<Node> children = new List<Node>();

    protected bool finishFlag = false;
    protected bool addedToMovementQueue = false;

    public Node(BlackBoard bb)
    {
        this.bb = bb;
    }

    public Node(BlackBoard bb, List<Node> nodes)
    {
        this.bb = bb;

        for (int i = 0; i < nodes.Count; i++)
            AttachChildNode(nodes[i]);
    }
    
    protected void AttachChildNode(Node node)
    {
        node.parent = this;
        children.Add(node);
    }

    // ��带 �����մϴ�. 
    public virtual BT_State Execute()
    {
        state = BT_State.Failure;
        return state;
    }
    
    public void SetFinishedFlag(bool isFinished)
    {
        finishFlag = isFinished;
    }

    public void SetFailureState()
    {
        state = BT_State.Failure;
    }

    protected void CheckFinishFlag()
    {
        // �� ��忡 �ش��ϴ� �������� ������ ���� �÷��װ� �ԷµǸ�, Success�� state�� �ٲ�ϴ�. 
        if (finishFlag)
        {
            finishFlag = false;
            addedToMovementQueue = false;
            state = BT_State.Success;
            return;
        }

        // ���� ���� �ʰ�, �������̸� state�� Running�� �˴ϴ�. 
        state = BT_State.Running;
    }
}

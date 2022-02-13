using System.Collections.Generic;


public enum BT_State
{
    Running,
    Success,
    Failure
}

// 행동 트리의 노드입니다. 노드는 크게 Selector, Sequence, Action들이 있습니다. 
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

    // 노드를 실행합니다. 
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
        // 이 노드에 해당하는 움직임이 끝나서 종료 플래그가 입력되면, Success로 state가 바뀝니다. 
        if (finishFlag)
        {
            finishFlag = false;
            addedToMovementQueue = false;
            state = BT_State.Success;
            return;
        }

        // 종료 되지 않고, 진행중이면 state가 Running이 됩니다. 
        state = BT_State.Running;
    }
}

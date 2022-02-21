using System.Collections.Generic;


public enum BT_State
{
    Running,
    Success,
    Failure
}

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
        if (finishFlag)
        {
            finishFlag = false;
            addedToMovementQueue = false;
            state = BT_State.Success;
            return;
        }

        state = BT_State.Running;
    }
}

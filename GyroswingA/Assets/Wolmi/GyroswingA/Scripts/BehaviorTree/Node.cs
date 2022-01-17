using System.Collections.Generic;


public enum BT_State
{
    Running,
    Success,
    Failure
}
public class Node
{
    protected bool finishFlag;
    protected bool addedToMovementQueue;

    protected BT_State state;
    protected Node parent;
    protected List<Node> children;
    protected BlackBoard bb;

    public Node(BlackBoard bb)
    {
        Init(bb);
    }

    public Node(BlackBoard bb, List<Node> nodes)
    {
        Init(bb);

        for (int i = 0; i < nodes.Count; i++)
        {
            AttachChildNode(nodes[i]);
        }
    }

    void Init(BlackBoard bb)
    {
        finishFlag = false;
        addedToMovementQueue = false;
        parent = null;
        children = new List<Node>();
        this.bb = bb;
    }

    protected void AttachChildNode(Node node)
    {
        // attach parent, when the node is added
        node.parent = this;
        children.Add(node);
    }

    public virtual BT_State Execute()
    {
        return BT_State.Failure;
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
            state = BT_State.Success;
            finishFlag = false;
            addedToMovementQueue = false;
        }
        else
        {
            state = BT_State.Running;
        }
    }

}

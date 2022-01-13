using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BT_State
{
    Running,
    Success,
    Failure
}

public class Node
{
    protected bool finishFlag;

    protected BT_State state;
    protected Node parent;
    protected List<Node> children;
    protected BlackBoard bb;

    public Node(BlackBoard bb)
    {
        finishFlag = false;
        parent = null;
        children = new List<Node>();
        this.bb = bb;
    }

    public Node(BlackBoard bb, List<Node> nodes)
    {
        finishFlag = false;
        parent = null;
        children = new List<Node>();
        this.bb = bb;

        for (int i = 0; i < nodes.Count; i++)
        {
            AttachChildNode(nodes[i]);
        }
    }

    protected void AttachChildNode(Node node)
    {
        // attach parent when added
        node.parent = this;
        children.Add(node);
    }

    public virtual BT_State Execute()
    {
        return state;
    }

    public void SetFinishedFlag(bool isFinished)
    {
        finishFlag = isFinished;
    }

    protected void CheckFinishFlag()
    {
        if (finishFlag)
        {
            state = BT_State.Success;
            finishFlag = false;
        }
        else
        {
            state = BT_State.Running;
        }
    }

}

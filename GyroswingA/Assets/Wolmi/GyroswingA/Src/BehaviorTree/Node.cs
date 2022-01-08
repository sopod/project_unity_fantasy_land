using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public enum BT_State
{
    Running,
    Success,
    Failure
}

public class Node
{
    protected BT_State state;
    protected Node parent;
    protected List<Node> children;
    protected BlackBoard bb;

    public Node(BlackBoard bb)
    {
        parent = null;
        children = new List<Node>();
        this.bb = bb;
    }

    public Node(BlackBoard bb, List<Node> nodes)
    {
        parent = null;
        children = new List<Node>();
        this.bb = bb;

        for (int i = 0; i < nodes.Count; i++)
        {
            AddNode(nodes[i]);
        }
    }

    void AddNode(Node node)
    {
        // attach parent when added
        node.parent = this;
        children.Add(node);
    }

    public virtual BT_State Execute()
    {
        return state;
    }
    
}

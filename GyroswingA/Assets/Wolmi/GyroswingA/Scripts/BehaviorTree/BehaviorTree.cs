using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public abstract class BehaviorTree : MovingThing
{
    protected BlackBoard bb;
    protected Node root;

    public void UpdateBT()
    {
        if (root != null && !IsStopped() && !IsPaused())
            root.Execute();
    }
    
    public abstract Node SetBT();
}


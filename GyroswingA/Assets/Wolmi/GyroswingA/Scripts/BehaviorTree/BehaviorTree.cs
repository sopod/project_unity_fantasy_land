using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public abstract class BehaviorTree : MovingThing
{
    protected BlackBoard bb;
    protected Options options;

    protected Node root;

    public abstract Node SetBT(Options options);

    public void UpdateBT()
    {
        if (root != null && !IsStopped() && !IsPaused())
            root.Execute();
    }
    
}


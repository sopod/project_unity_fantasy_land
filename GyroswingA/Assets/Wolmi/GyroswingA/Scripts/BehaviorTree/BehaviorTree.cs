

public abstract class BehaviorTree : MovingThing
{
    protected BlackBoard bb;
    protected Node root;

    public abstract Node SetBT(Layers layer);

    public void UpdateBT()
    {
        if (root == null || IsStopped || IsPaused) return;

        root.Execute();
    }    
}


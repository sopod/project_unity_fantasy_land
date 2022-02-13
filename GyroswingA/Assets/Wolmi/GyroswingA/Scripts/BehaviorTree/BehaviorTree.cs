


public abstract class BehaviorTree : MovingThing
{
    protected BlackBoard bb;
    protected Node root;

    // 행동 트리를 처음 설정합니다. 
    public abstract Node SetBT(Layers layer, ObjectValues values);

    // 행동 트리를 계속 Execute하여 업데이트합니다. 
    public void UpdateBT()
    {
        if (root == null || IsStopped || IsPaused) return;

        root.Execute();
    }    
}


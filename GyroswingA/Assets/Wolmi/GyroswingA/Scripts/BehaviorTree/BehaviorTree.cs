


public abstract class BehaviorTree : MovingThing
{
    protected BlackBoard bb;
    protected Node root;

    // �ൿ Ʈ���� ó�� �����մϴ�. 
    public abstract Node SetBT(Layers layer, ObjectValues values);

    // �ൿ Ʈ���� ��� Execute�Ͽ� ������Ʈ�մϴ�. 
    public void UpdateBT()
    {
        if (root == null || IsStopped || IsPaused) return;

        root.Execute();
    }    
}


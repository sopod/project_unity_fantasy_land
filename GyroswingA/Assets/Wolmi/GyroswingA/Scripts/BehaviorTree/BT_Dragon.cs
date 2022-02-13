using System.Collections.Generic;


public class BT_Dragon : BehaviorTree
{
    public override Node SetBT(Layers layer, ObjectValues values)
    {
        bb = new BlackBoard(this.gameObject.GetComponent<Enemy>(), layer, values);

        root = new Selector(bb, new List<Node>
                {
                    // �÷��̾ ��ô �����̿� �ִٸ�, �����ϰ� ����մϴ�. 
                    new Sequence(bb, new List<Node>
                    {
                        new Action_CheckPlayerToAttack(bb),
                        new Action_Attack(bb),
                        new Action_Wait(bb)
                    }),

                    // �÷��̾ �����̿� �ִٸ�, �߰��ϰ� ����մϴ�. 
                    new Sequence(bb, new List<Node>
                    {
                        new Action_CheckPlayerAround(bb),
                        new Action_MoveToPlayer(bb),
                        new Action_Wait(bb),
                    }),

                    // �÷��̾ ã�� ���Ͽ��ٸ�, �����ϰ� ����մϴ�. 
                    new Sequence(bb, new List<Node>
                    {
                        new Action_Patrol(bb),
                        new Action_TurnBack(bb),
                        new Action_Wait(bb)
                    })
                }
            );     
        
        StartMoving();
        return root;
    }    
}


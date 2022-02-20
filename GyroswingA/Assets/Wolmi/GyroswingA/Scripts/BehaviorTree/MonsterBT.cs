using System.Collections.Generic;
using UnityEngine;

public class MonsterBT : BehaviorTree
{
    public override Node SetBT(Layers layer, ObjectValues values, Transform player)
    {
        bb = new BlackBoard(this.gameObject.GetComponent<Enemy>(), layer, values, player);

        root = new Selector(bb, new List<Node>
                {
                    // �÷��̾ ��ô �����̿� �ִٸ�, �����ϰ� ����մϴ�. 
                    new Sequence(bb, new List<Node>
                    {
                        new ActionNodes.CheckPlayerToAttack(bb),
                        new ActionNodes.Attack(bb),
                        new ActionNodes.Wait(bb)
                    }),

                    // �÷��̾ �����̿� �ִٸ�, �߰��ϰ� ����մϴ�. 
                    new Sequence(bb, new List<Node>
                    {
                        new ActionNodes.CheckPlayerAround(bb),
                        new ActionNodes.MoveToPlayer(bb),
                        new ActionNodes.Wait(bb),
                    }),

                    // �÷��̾ ã�� ���Ͽ��ٸ�, �����ϰ� ����մϴ�. 
                    new Sequence(bb, new List<Node>
                    {
                        new ActionNodes.Patrol(bb),
                        new ActionNodes.TurnBack(bb),
                        new ActionNodes.Wait(bb)
                    })
                }
            );     
        
        //StartMoving();
        return root;
    }    
}


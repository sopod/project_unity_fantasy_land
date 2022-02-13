using System.Collections.Generic;


public class BT_Dragon : BehaviorTree
{
    public override Node SetBT(Layers layer, ObjectValues values)
    {
        bb = new BlackBoard(this.gameObject.GetComponent<Enemy>(), layer, values);

        root = new Selector(bb, new List<Node>
                {
                    // 플레이어가 무척 가까이에 있다면, 공격하고 대기합니다. 
                    new Sequence(bb, new List<Node>
                    {
                        new Action_CheckPlayerToAttack(bb),
                        new Action_Attack(bb),
                        new Action_Wait(bb)
                    }),

                    // 플레이어가 가까이에 있다면, 추격하고 대기합니다. 
                    new Sequence(bb, new List<Node>
                    {
                        new Action_CheckPlayerAround(bb),
                        new Action_MoveToPlayer(bb),
                        new Action_Wait(bb),
                    }),

                    // 플레이어를 찾지 못하였다면, 정찰하고 대기합니다. 
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


using System.Collections.Generic;
using UnityEngine;

public class MonsterBT : BehaviorTree
{
    public override Node SetBT(Layers layer, ObjectValues values, Transform player)
    {
        bb = new BlackBoard(this.gameObject.GetComponent<Enemy>(), layer, values, player);

        root = new Selector(bb, new List<Node>
                {
                    // 플레이어가 무척 가까이에 있다면, 공격하고 대기합니다. 
                    new Sequence(bb, new List<Node>
                    {
                        new ActionNodes.CheckPlayerToAttack(bb),
                        new ActionNodes.Attack(bb),
                        new ActionNodes.Wait(bb)
                    }),

                    // 플레이어가 가까이에 있다면, 추격하고 대기합니다. 
                    new Sequence(bb, new List<Node>
                    {
                        new ActionNodes.CheckPlayerAround(bb),
                        new ActionNodes.MoveToPlayer(bb),
                        new ActionNodes.Wait(bb),
                    }),

                    // 플레이어를 찾지 못하였다면, 정찰하고 대기합니다. 
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


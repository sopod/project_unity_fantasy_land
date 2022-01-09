using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Dragon : BehaviorTree
{
    public override Node SetBT()
    {
        bb = new BlackBoard(this.gameObject);

        root = 
            new Selector(bb, new List<Node>
                {
                    new Sequence(bb, new List<Node>
                        {
                            new Action_CheckPlayerAround(bb),
                            new Action_MoveToPlayer(bb)
                        }),
                    new Action_Patrol(bb)
                }
            );
        
        StartMoving();

        return root;
    }
    
}

using System.Collections.Generic;



public class BT_Dragon : BehaviorTree
{
    public override Node SetBT(Options options)
    {
        this.options = options;
        bb = new BlackBoard(this.gameObject.GetComponent<Enemy>(), options);

        root = 
            new Selector(bb, new List<Node>
                {
                    // check if player is very near. If so, attack
                    new Sequence(bb, new List<Node>
                    {
                        new Action_CheckPlayerToAttack(bb),
                        new Action_Attack(bb),
                        new Action_Wait(bb),
                        new Action_Wait(bb),
                        new Action_Wait(bb)
                    }),

                    // check if player is near. If so, chase
                    new Sequence(bb, new List<Node>
                    {
                        new Action_CheckPlayerAround(bb),
                        new Action_MoveToPlayer(bb),
                        new Action_Wait(bb),
                        new Action_Wait(bb)
                    }),

                    // check if enemy can't find player. If so, patrol
                    new Sequence(bb, new List<Node>
                    {
                        new Action_Patrol(bb),
                        new Action_Wait(bb),
                        new Action_TurnBack(bb),
                        new Action_Wait(bb)
                    })
                }
            );     
        
        StartMoving();
        return root;
    }    
}


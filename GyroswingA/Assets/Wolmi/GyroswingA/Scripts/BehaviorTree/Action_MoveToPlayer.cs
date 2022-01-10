using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_MoveToPlayer : Node
{
    public Action_MoveToPlayer(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        Vector3 playerPos = GameManager.Instance.PlayerPosition;
        
        bb.character.transform.LookAt(playerPos); // look at player

        if (Physics.Raycast(bb.character.transform.position, playerPos, bb.rayDistance, bb.stagePoleLayer)) // if pole is in front of monster
        {
            
        }
        else
        {
            
        }

        bb.character.GetComponent<EnemyController>().AddEnemyMovement(EnemyMovement.MoveForward, this);

        CheckFinishFlag();
        return state;
    }
    
}

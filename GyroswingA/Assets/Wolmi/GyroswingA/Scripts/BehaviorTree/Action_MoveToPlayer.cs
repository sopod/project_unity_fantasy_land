using UnityEngine;

public class Action_MoveToPlayer : Node
{
    public Action_MoveToPlayer(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        if (!addedToMovementQueue)
        {
            addedToMovementQueue = true;

            Vector3 playerPos = GameManager.Instance.PlayerPosition;

            bb.character.transform.LookAt(playerPos); // look at player
            
            bb.character.GetComponent<EnemyController>().AddEnemyMovement(new MovementData(EnemyMovement.MoveForward, this, bb.enemyMoveTime));
        }

        CheckFinishFlag();
        return state;
    }
    
}

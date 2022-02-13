using UnityEngine;


public class Action_MoveToPlayer : Node
{
    public Action_MoveToPlayer(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        // 플레이어를 위치를 바라본 후, 앞으로 전진합니다. 
        if (!addedToMovementQueue)
        {
            addedToMovementQueue = true;
            Vector3 playerPos = GameCenter.Instance.PlayerPosition;

            bb.OwnerCharacter.transform.LookAt(playerPos);            
            bb.OwnerCharacter.AddEnemyMovement(new MovementData(EnemyMovement.MoveForward, this, bb.EnemyMoveTime));
        }

        CheckFinishFlag();
        return state;
    }
    
}

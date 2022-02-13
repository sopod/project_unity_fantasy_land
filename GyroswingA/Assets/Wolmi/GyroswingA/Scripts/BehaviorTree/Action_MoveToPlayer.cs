using UnityEngine;


public class Action_MoveToPlayer : Node
{
    public Action_MoveToPlayer(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        // �÷��̾ ��ġ�� �ٶ� ��, ������ �����մϴ�. 
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

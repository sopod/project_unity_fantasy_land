using UnityEngine;


public class Action_Attack : Node
{
    public Action_Attack(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        // �÷��̾� ��ġ�� �ٶ� ��, �÷��̾ �����մϴ�. 
        Vector3 playerPos = GameCenter.Instance.PlayerPosition;
        bb.OwnerCharacter.transform.LookAt(playerPos); 
        bb.OwnerCharacter.AttackPlayer();

        state = BT_State.Success;
        return state;
    }
}

using UnityEngine;


public class Action_Attack : Node
{
    public Action_Attack(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        // 플레이어 위치를 바라본 후, 플레이어를 공격합니다. 
        Vector3 playerPos = GameCenter.Instance.PlayerPosition;
        bb.OwnerCharacter.transform.LookAt(playerPos); 
        bb.OwnerCharacter.AttackPlayer();

        state = BT_State.Success;
        return state;
    }
}

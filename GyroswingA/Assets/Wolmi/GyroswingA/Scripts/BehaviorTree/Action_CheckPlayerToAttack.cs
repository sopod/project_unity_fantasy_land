using UnityEngine;


public class Action_CheckPlayerToAttack : Node
{
    public Action_CheckPlayerToAttack(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        // 플레이어가 PlayerAttackRadius 안에 있는지 판단합니다. 
        Collider[] hitColliders = Physics.OverlapSphere(bb.OwnerCharacter.transform.position, bb.PlayerAttackRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if ((1 << hitColliders[i].gameObject.layer) == bb.Layers.PlayerLayer)
            {
                state = BT_State.Success;
                return state;
            }
        }

        state = BT_State.Failure;
        return state;
    }
}
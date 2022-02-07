using UnityEngine;

public class Action_CheckPlayerToAttack : Node
{
    public Action_CheckPlayerToAttack(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        Collider[] hitColliders = Physics.OverlapSphere(bb.character.transform.position, bb.playerAttackRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if ((1 << hitColliders[i].gameObject.layer) == bb.layerStruct.PlayerLayer)
            {
                state = BT_State.Success;
                return state;
            }
        }

        state = BT_State.Failure;
        return state;
    }
}
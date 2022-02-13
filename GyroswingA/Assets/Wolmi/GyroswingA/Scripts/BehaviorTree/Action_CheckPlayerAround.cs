using UnityEngine;


public class Action_CheckPlayerAround : Node
{
    public Action_CheckPlayerAround(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        // �÷��̾ PlayerDetectionRadius �ȿ� �ִ��� �Ǵ��մϴ�. 
        Collider[] hitColliders = Physics.OverlapSphere(bb.OwnerCharacter.transform.position, bb.PlayerDetectionRadius);

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

using UnityEngine;

namespace ActionNodes
{ 
    public class CheckPlayerToAttack : Node
    {
        public CheckPlayerToAttack(BlackBoard bb) : base(bb) { }

        public override BT_State Execute()
        {
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
}
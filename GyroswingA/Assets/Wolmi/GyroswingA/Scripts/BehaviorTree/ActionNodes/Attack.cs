using UnityEngine;

namespace ActionNodes
{ 
    public class Attack : Node
    {
        public Attack(BlackBoard bb) : base(bb) { }

        public override BT_State Execute()
        {
            bb.OwnerCharacter.transform.LookAt(bb.player.position); 
            bb.OwnerCharacter.AttackPlayer();

            state = BT_State.Success;
            return state;
        }
    }
}

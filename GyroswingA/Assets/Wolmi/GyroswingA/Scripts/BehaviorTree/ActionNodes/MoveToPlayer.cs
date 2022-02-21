using UnityEngine;

namespace ActionNodes
{ 
    public class MoveToPlayer : Node
    {
        public MoveToPlayer(BlackBoard bb) : base(bb) { }

        public override BT_State Execute()
        {
            if (!addedToMovementQueue)
            {
                addedToMovementQueue = true;
                bb.OwnerCharacter.transform.LookAt(bb.player.position);            
                bb.OwnerCharacter.AddMovement(new MovementData(EnemyMovement.MoveForward, this, bb.EnemyMoveTime));
            }

            CheckFinishFlag();
            return state;
        }
    }
}


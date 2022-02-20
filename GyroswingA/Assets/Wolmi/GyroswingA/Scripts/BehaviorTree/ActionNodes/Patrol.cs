using UnityEngine;

namespace ActionNodes
{ 
    public class Patrol : Node
    {
        public Patrol(BlackBoard bb) : base(bb) { }

        public override BT_State Execute()
        {
            if (!addedToMovementQueue)
            {
                addedToMovementQueue = true;
                int ranNum = Random.Range(0, 2);
                EnemyMovement move = (ranNum == 0) ? EnemyMovement.TurnLeft : EnemyMovement.TurnRight;

                bb.OwnerCharacter.AddMovement(new MovementData(move, null, bb.EnemyTurnTime));
                bb.OwnerCharacter.AddMovement(new MovementData(EnemyMovement.MoveForward, this, bb.EnemyMoveTime));
            }

            CheckFinishFlag();
            return state;
        }
    }
}
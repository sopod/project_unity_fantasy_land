using UnityEngine;

namespace ActionNodes
{ 
    public class TurnBack : Node
    {
        public TurnBack(BlackBoard bb) : base(bb) { }

        public override BT_State Execute()
        {
            if (!addedToMovementQueue)
            {
                addedToMovementQueue = true;
                int ranNum = Random.Range(0, 2);
                EnemyMovement move = (ranNum == 0) ? EnemyMovement.TurnLeft : EnemyMovement.TurnRight;

                bb.OwnerCharacter.AddMovement(new MovementData(move, this, bb.EnemyLongTurnTime));
            }

            CheckFinishFlag();
            return state;
        }
    }
}


namespace ActionNodes
{ 
    public class Wait : Node
    {
        public Wait(BlackBoard bb) : base(bb) { }

        public override BT_State Execute()
        {
            if (!addedToMovementQueue)
            {
                addedToMovementQueue = true;
                bb.OwnerCharacter.AddMovement(new MovementData(EnemyMovement.Wait, this, bb.EnemyWaitTime));
            }

            CheckFinishFlag();

            return state;
        }
    }
}
using UnityEngine;

public class Action_Attack : Node
{
    public Action_Attack(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        Vector3 playerPos = GameCenter.Instance.PlayerPosition;

        bb.character.transform.LookAt(playerPos); // look at player

        bb.character.GetComponent<EnemyController>().AttackPlayer(); // attack player

        state = BT_State.Success;

        return state;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_CheckPlayerAround : Node
{
    public Action_CheckPlayerAround(BlackBoard bb) : base(bb) { }

    public override BT_State Execute()
    {
        Collider[] hitColliders = Physics.OverlapSphere(bb.character.transform.position, bb.playerDetectionRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        { 
            if ((1 << hitColliders[i].gameObject.layer) == bb.playerLayer)
            {
                Debug.Log("I found player");
                state =  BT_State.Success;
                return state;
            }
        }

        state = BT_State.Failure;
        return state;
    }


}

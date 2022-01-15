using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard
{
    public GameObject character;
    public Options options;

    // detection range
    public float playerDetectionRadius { get { return 2.0f; } }
    public float playerAttackRadius { get { return 0.5f; } }
    public float rayDistance { get { return 1.0f; } }


    // time duration
    public float enemyWaitTime { get { return 1.0f; } }
    public float enemyTurnTime { get { return 0.3f; } }
    public float enemyMoveTime { get { return 0.3f; } }
    public float enemyLongTurnTime { get { return 0.5f; } }


    public BlackBoard(GameObject monster, Options options)
    {
        this.options = options;
        character = monster;
    }
    


}

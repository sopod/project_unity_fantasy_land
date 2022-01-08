using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard
{
    public GameObject character;
    public LayerMask playerLayer;
    public LayerMask stagePoleLayer;

    public float playerDetectionRadius { get { return 1.0f; } }
    public float playerAttackRadius { get { return 0.3f; } }
    public float rayDistance { get { return 1.0f; } }

    public BlackBoard(GameObject monster)
    {
        character = monster;
        playerLayer = GameManager.Instance.playerLayer;
        stagePoleLayer = GameManager.Instance.stagePoleLayer;
    }
    


}

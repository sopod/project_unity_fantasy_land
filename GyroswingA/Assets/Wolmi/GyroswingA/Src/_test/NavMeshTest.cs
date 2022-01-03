using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    [SerializeField] bool makeActive = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameManager.Instance.player.gameObject;
    }

    void Update()
    {
        if (makeActive)
            agent.SetDestination(player.transform.position);
    }
}

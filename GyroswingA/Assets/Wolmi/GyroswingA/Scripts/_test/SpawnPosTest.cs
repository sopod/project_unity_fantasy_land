using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosTest : MonoBehaviour
{
    [SerializeField] GameObject toSpawn;

    void Start()
    {
        GameObject g = Instantiate(toSpawn, transform.position, transform.rotation, null);

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 10.0f))
        {
            g.transform.position = hit.point + new Vector3(0, toSpawn.transform.position.y, 0);
        }
        else
        {
            Debug.Log("no no no");
        }
    }
    
}

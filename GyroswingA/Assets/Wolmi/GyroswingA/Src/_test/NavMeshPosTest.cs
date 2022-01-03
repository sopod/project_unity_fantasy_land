using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshPosTest : MonoBehaviour
{
    [SerializeField] GameObject stage;

    void Update()
    {
        Vector3 moveVec = stage.transform.position;
        transform.position = moveVec;
    }
}

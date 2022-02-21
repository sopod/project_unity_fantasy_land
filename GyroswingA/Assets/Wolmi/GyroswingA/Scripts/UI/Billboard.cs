using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [HideInInspector] public Transform playerCamera;

    void LateUpdate()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.LookAt(transform.position + playerCamera.forward);
        Quaternion quat = Quaternion.FromToRotation(transform.right, playerCamera.right);
        transform.rotation = quat * transform.rotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [HideInInspector] public Transform camera;

    void LateUpdate()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.LookAt(transform.position + camera.forward);
        Quaternion quat = Quaternion.FromToRotation(transform.right, camera.right);
        transform.rotation = quat * transform.rotation;
    }
}

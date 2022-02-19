using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform camDir;
    float camDistance = 2.5f;

    private Ray camRay;
    public RaycastHit CamRayHit;
    [SerializeField] LayerMask layer;
    float distance = 5f;
    float radius = 0.1f;

    void LateUpdate()
    {
        MoveCameraWithRayCast();
        RotateCamera();
    }

    void MoveCameraWithRayCast()
    {
        camRay.origin = camDir.position;
        camRay.direction = GetVectorFromPlayerToCamera();

        if (Physics.SphereCast(camRay, radius, out CamRayHit, distance))
            transform.position = CamRayHit.point + CamRayHit.normal * 0.1f;
        else
            transform.position = target.position + GetVectorFromPlayerToCamera() * camDistance;
    }

    void RotateCamera()
    {
        transform.LookAt(camDir);

        Quaternion quat = Quaternion.FromToRotation(transform.right, target.right);
        transform.rotation = quat * transform.rotation;
    }

    Vector3 GetVectorFromPlayerToCamera()
    {
        return (camDir.position - target.position).normalized;
    }
}

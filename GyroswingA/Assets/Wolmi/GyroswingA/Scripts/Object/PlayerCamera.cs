using UnityEngine;


public class PlayerCamera : MonoBehaviour
{
    const float CAMERA_DISTANCE = 2.2f;

    [SerializeField] Transform target;
    [SerializeField] Transform camDir;

    private Ray camRay;
    public RaycastHit CamRayHit;
    [SerializeField] LayerMask layer;
    float rayDistance = CAMERA_DISTANCE;
    float rayRadius = 0.1f;

    void LateUpdate()
    {
        MoveCameraWithRayCast();
        RotateCamera();
    }

    void MoveCameraWithRayCast()
    {
        camRay.origin = camDir.position;
        camRay.direction = GetVectorFromPlayerToCamera();

        if (Physics.SphereCast(camRay, rayRadius, out CamRayHit, rayDistance, layer))
            transform.position = CamRayHit.point + CamRayHit.normal * 0.1f;
        else
            transform.position = target.position + GetVectorFromPlayerToCamera() * CAMERA_DISTANCE;
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

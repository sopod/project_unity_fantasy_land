using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] LayerMask stageLayer;
    [SerializeField] GameObject cameraCenter;
    [SerializeField] GameObject cameraCollider;
    [SerializeField] Camera camera;
    RaycastHit camHit;

    Vector3 cameraCenterOriginalLocalPos = new Vector3(0.0f, 1.0f, 0.0f);
    Vector3 cameraOriginalLocalPos = new Vector3(0.0f, 6.0f, -8.0f);
    Vector3 cameraOriginalLocalRot = new Vector3(20.0f, 0.0f, 0.0f);

    float collisionSensivitity = 4.5f;

    void Start()
    {
        cameraCenter.transform.localPosition = cameraCenterOriginalLocalPos;
        camera.transform.localPosition = cameraOriginalLocalPos;
        camera.transform.localRotation = Quaternion.Euler(cameraOriginalLocalRot);

        cameraCollider.transform.localPosition = camera.transform.localPosition - new Vector3(0.0f, 0.0f, collisionSensivitity);
    }

    void Update()
    {
        cameraCollider.transform.localPosition = camera.transform.localPosition - new Vector3(0.0f, 0.0f, collisionSensivitity);

        if (Physics.Linecast(cameraCenter.transform.position, cameraCollider.transform.position, out camHit, stageLayer))
        {
            camera.transform.position = camHit.point;


            Debug.DrawLine(cameraCenter.transform.position, cameraCollider.transform.position, Color.red);

            //Vector3 localPosition = camera.transform.position + new Vector3(0.0f, 0.0f, collisionSensivitity);
            //camera.transform.localPosition = localPosition;
        }

        //if (camera.transform.localPosition.z > -1f)
        //{
        //    camera.transform.localPosition = new Vector3(camera.transform.localPosition.y, -1f);
        //}
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(cameraCenter.transform.position, 1f);
        Gizmos.DrawSphere(camHit.point, 1f);
    }
}

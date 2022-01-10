using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Rigidbody rb;
    KeyController key;
    [SerializeField] GameObject stage;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        key = new KeyController();



        //transform.rotation *= Quaternion.AngleAxis(30.0f, Vector3.right);
        transform.rotation *= Quaternion.AngleAxis(30.0f, -Vector3.up);
    }

    void Update()
    {


        //transform.rotation *= Quaternion.Euler(0.0f, 1.0f, 0.0f);

        //// rotate
        //float angle = key.GetHorizontalKey() * 130.0f * Time.fixedDeltaTime;
        //rb.rotation = transform.localRotation * Quaternion.AngleAxis(angle, Vector3.up);

        //// move
        //Vector3 moveVec = stage.transform.forward * key.GetVerticalKey() * 10.0f * Time.deltaTime;
        //rb.MovePosition(rb.position + moveVec);
    }
}

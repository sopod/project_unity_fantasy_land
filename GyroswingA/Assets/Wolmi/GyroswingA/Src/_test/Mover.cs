using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    [SerializeField] GameObject stage;
    public Rigidbody rb;

    public bool isJumping;
    public bool isOnStage;

    public float gravity;
    public float jumpPower;
    public float moveSpeed;
    public float rotSpeed;

    protected void Set()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        isJumping = false;
        isOnStage = false;

        gravity = 9.8f;
        jumpPower = 10.0f;
        moveSpeed = 1.0f;
        rotSpeed = 90.0f;
    }

    protected void AffectedByGravity()
    {
        if (isOnStage)
        {
            rb.velocity -= stage.transform.up * gravity * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity -= Vector3.up * gravity * Time.fixedDeltaTime;
        }
    }
}

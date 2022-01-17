using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Mover
{
    KeyController key;
    
    void Start()
    {
        Set();
    }

    void Update()
    {
        Rotate();
        Move();
        Jump();
        AffectedByGravity();
    }

    void Rotate()
    {
        float angle = key.GetHorizontalKey() * rotSpeed * Time.fixedDeltaTime;
        rb.rotation *= Quaternion.AngleAxis(angle, Vector3.up);
    }

    void Move()
    {
        Vector3 moveVec = transform.forward * key.GetVerticalKey() * moveSpeed * Time.fixedDeltaTime;
        rb.position += moveVec;
    }

    void Jump()
    {
        if (!isJumping && key.IsJumpKeyPressed())
        {
            rb.velocity += transform.up * jumpPower;
            isJumping = true;
        }
    }

}

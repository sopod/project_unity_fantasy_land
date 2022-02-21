using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationState
{
    Null,
    Dead, 
    Jump,
    Dash, 
    Fire, 
    Move, 
    Max
}


public class CreatureAnimation
{
    Animator ani;
    AnimationState state;

    bool isMoving = false;
    bool isTurning = false;

    public CreatureAnimation(Animator ani)
    {
        this.ani = ani;
    }

    public void InitAnimation()
    {
        ani.SetFloat("MoveFront", 0.0f);
        ani.SetFloat("TurnRight", 0.0f);
        ani.SetBool("IsMoving", false);
        ani.SetBool("IsJumping", false);
        ani.SetBool("IsDead", false);
    }

    public void DoIdleAnimation()
    {
        isMoving = false;
        isTurning = false;

        EndJumpAnimation();
    }

    public void SetMoveAnimation(float key)
    {
        if ((Mathf.Abs(key) > 0.1f)) isMoving = true;
        else isMoving = false;

        ani.SetFloat("MoveFront", key);
    }

    public void SetTurnAnimation(float key)
    {
        if ((Mathf.Abs(key) > 0.1f)) isTurning = true;
        else isTurning = false;

        ani.SetFloat("TurnRight", key);
    }

    public void DoMovingAnimation()
    {
        ani.SetBool("IsMoving", isMoving || isTurning);
    }

    public void DoJumpAnimation()
    {
        ani.SetBool("IsJumping", true);
    }

    public void EndJumpAnimation()
    {
        ani.SetBool("IsJumping", false);
    }

    public void DoDashAnimation()
    {
        ani.SetTrigger("JustDashed");
    }

    public void DoFireAnimation()
    {
        ani.SetTrigger("JustFired");
    }

    public void DoDeadAnimation()
    {
        ani.SetBool("IsDead", true);
    }

}

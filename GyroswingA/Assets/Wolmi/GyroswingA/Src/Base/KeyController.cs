using UnityEngine;

public class KeyController
{
    KeyCode moveFrontKey = KeyCode.W;
    KeyCode moveBackKey = KeyCode.S;
    KeyCode moveRightKey = KeyCode.D;
    KeyCode moveLeftKey = KeyCode.A;
    KeyCode jumpKey = KeyCode.Space;
    KeyCode dashKey = KeyCode.Q;

    string mouseX = "Mouse X";
    string mouseY = "Mouse Y";

    public float GetHorizontalKey()
    {
        if (Input.GetKey(moveRightKey))
            return 1.0f;
        if (Input.GetKey(moveLeftKey))
            return -1.0f;
        return 0.0f;
    }

    public float GetVerticalKey()
    {
        if (Input.GetKey(moveFrontKey))
            return 1.0f;

        if (Input.GetKey(moveBackKey))
            return -1.0f;
        return 0.0f;
    }

    public float GetMouseX()
    {
        return Input.GetAxis(mouseX);
    }

    public float GetMouseY()
    {
        return Input.GetAxis(mouseY);
    }

    public bool IsJumpKeyPressed()
    {
        return Input.GetKey(jumpKey);
    }

    public bool IsDashKeyPressed()
    {
        return Input.GetKey(dashKey);
    }
}

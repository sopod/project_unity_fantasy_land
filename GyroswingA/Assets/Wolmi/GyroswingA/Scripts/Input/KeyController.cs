using UnityEngine;



public class KeyController
{
    JoystickController joystick;

    const KeyCode moveFrontKey = KeyCode.W;
    const KeyCode moveBackKey = KeyCode.S;
    const KeyCode moveRightKey = KeyCode.D;
    const KeyCode moveLeftKey = KeyCode.A;
    const KeyCode jumpKey = KeyCode.Space;
    const KeyCode dashKey = KeyCode.Q;
    const KeyCode fireKey = KeyCode.E;

    const string mouseX = "Mouse X";
    const string mouseY = "Mouse Y";


    public KeyController(JoystickController joystick)
    {
        this.joystick = joystick;
    }

    public float GetHorizontalKey()
    {
        if (joystick.IsInput) return joystick.GetXDir();

        if (Input.GetKey(moveRightKey)) return 1.0f;

        if (Input.GetKey(moveLeftKey)) return -1.0f;

        return 0.0f;
    }

    public float GetVerticalKey()
    {
        if (joystick.IsInput) return joystick.GetYDir();
        
        if (Input.GetKey(moveFrontKey)) return 1.0f;

        if (Input.GetKey(moveBackKey)) return -1.0f;

        return 0.0f;
    }

    public bool IsJumpKeyPressed()
    {
        return Input.GetKey(jumpKey);
    }

    public bool IsDashKeyPressed()
    {
        return Input.GetKey(dashKey);
    }

    public bool IsFireKeyPressed()
    {
        return Input.GetKey(fireKey);
    }
    public float GetMouseX()
    {
        return Input.GetAxis(mouseX);
    }

    public float GetMouseY()
    {
        return Input.GetAxis(mouseY);
    }
}

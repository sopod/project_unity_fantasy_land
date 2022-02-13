using UnityEngine;


// 키보드 조작 및 터치 조작에 대한 입력값을 얻을 수 있는 KeyController 클래스를 따로 만들어 사용하였습니다. 


public class KeyController
{
    JoystickController joystick;

    KeyCode moveFrontKey = KeyCode.W;
    KeyCode moveBackKey = KeyCode.S;
    KeyCode moveRightKey = KeyCode.D;
    KeyCode moveLeftKey = KeyCode.A;
    KeyCode jumpKey = KeyCode.Space;
    KeyCode dashKey = KeyCode.Q;
    KeyCode fireKey = KeyCode.E;

    string mouseX = "Mouse X";
    string mouseY = "Mouse Y";


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

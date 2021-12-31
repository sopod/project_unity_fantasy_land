using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumPivot : MonoBehaviour
{
    [SerializeField] GameObject stage;

    float swingSpeed = 0.2f;
    float swingAngleMax = 35.0f;

    Quaternion _startRot, _endRot;
    float _curTime;

    void Start()
    {
        SetPendulum(swingAngleMax);
        ResetTimer();
    }

    void FixedUpdate()
    {
        Swing();
    }

    void SetPendulum(float angleMax)
    {
        _startRot = GetRotation(-angleMax);
        _endRot = GetRotation(angleMax);
    }

    void ResetTimer()
    {
        _curTime = (-Mathf.PI / 2) / swingSpeed;
    }

    Quaternion GetRotation(float angle)
    {
        Quaternion resRot = transform.rotation;
        float angleX = resRot.eulerAngles.x + angle;

        if (angleX > 180) angleX -= 360;
        else if (angleX < -180) angleX += 360;

        resRot.eulerAngles = new Vector3(angleX, resRot.eulerAngles.y, resRot.eulerAngles.z);

        return resRot;
    }

    void Swing()
    {
        _curTime += Time.deltaTime;

        transform.rotation = Quaternion.Lerp(_startRot, _endRot,
                                            (Mathf.Sin(_curTime * swingSpeed + Mathf.PI / 2) + 1.0f) / 2.0f);
    }
}

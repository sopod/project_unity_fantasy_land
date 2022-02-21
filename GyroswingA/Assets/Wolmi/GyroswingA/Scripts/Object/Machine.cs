using System;
using UnityEngine;


public class StageMovementValue
{
    public bool IsMachineSwinging;
    public bool IsMachineSpining;
    public bool IsSpiningCW;
    public Vector3 SwingPosCur;
    public float SpinAngleCur;
}


public class Machine : MovingThing
{
    public Action OnMachineMoved;

    StageMovementValue stageVal;
    StageChanger options;

    [SerializeField] GameObject swingBar;
    [SerializeField] GameObject stage;

    Vector3 stageStartPos;
    Quaternion stageStartRot;
    Vector3 barStartPos;
    Quaternion barStartRot;

    bool isMachineSwinging = true;
    bool isMachineTurning = true;
    bool isMachineSpining = true;
    bool isSpiningCW = true;

    float swingRadius;

    Vector3 swingPosCur;
    bool changeDir = false;
    bool isSwingingRight = true;
    float swingAngleCur = 0.0f;
    float swingAngleTotal = 0.0f;
    float spinAngleCur = 0.0f;


    void FixedUpdate()
    {
        if (IsPaused) return;

        SetSwingAngleCur();

        if (isMachineSwinging)
        {
            SwingBar();
            SwingStage();
        }

        if (isMachineTurning)
            TurnStage();

        if (isMachineSpining)
            SpinStage();

        if (isMachineSwinging)
            ChangeDirection();

        SetStageValues();
        OnMachineMoved?.Invoke();
    }

    public void Init(StageChanger stageChanger, StageMovementValue stageVal)
    {
        stageStartPos = stage.transform.position;
        stageStartRot = stage.transform.rotation;
        barStartPos = swingBar.transform.position;
        barStartRot = swingBar.transform.rotation;
                
        swingRadius = (swingBar.transform.position.y - stage.transform.position.y);

        this.stageVal = stageVal;
        this.options = stageChanger;

        PauseMoving();
    }

    public void ResetMachine()
    {
        stage.transform.position = stageStartPos;
        stage.transform.rotation = stageStartRot;
        swingBar.transform.position = barStartPos;
        swingBar.transform.rotation = barStartRot;

        spinAngleCur = 0.0f;
        changeDir = false;
        isSwingingRight = true;
        swingAngleCur = 0.0f;
        swingAngleTotal = 0.0f;
    }
    

    void SetSwingAngleCur()
    {
        swingAngleCur = options.GetCurrentStageValue().SwingSpeed * Time.fixedDeltaTime;

        if (isSwingingRight) swingAngleTotal += swingAngleCur;
        else                 swingAngleTotal -= swingAngleCur;

        if (swingAngleTotal >= 360.0f)     swingAngleTotal -= 360.0f;
        else if (swingAngleTotal <= 0.0f)  swingAngleTotal += 360.0f;

        // swingAngleMax <= swingAngleTotal <= 360.0f - swingAngleMax
        if ((options.GetCurrentStageValue().SwingAngleMax < swingAngleTotal) && (swingAngleTotal < (360.0f - options.GetCurrentStageValue().SwingAngleMax)))
        {
            changeDir = true;

            if (swingAngleTotal <= 180.0f)
            {
                swingAngleCur -= swingAngleTotal - options.GetCurrentStageValue().SwingAngleMax;
                swingAngleTotal = options.GetCurrentStageValue().SwingAngleMax;
            }
            else
            {
                swingAngleCur -= (360.0f - options.GetCurrentStageValue().SwingAngleMax) - swingAngleTotal;
                swingAngleTotal = 360.0f - options.GetCurrentStageValue().SwingAngleMax;
            }
        }        
    }

    void SwingBar()
    {
        if (isSwingingRight)  swingBar.transform.Rotate(Vector3.left, swingAngleCur, Space.World);
        else                  swingBar.transform.Rotate(-Vector3.left, swingAngleCur, Space.World);
    }

    void SwingStage()
    {
        float radian = Mathf.Deg2Rad * swingAngleTotal;

        swingPosCur.z = (Mathf.Sin(radian) * swingRadius) + stageStartPos.z - stage.transform.position.z;
        swingPosCur.y = (swingRadius - Mathf.Cos(radian) * swingRadius) + stageStartPos.y - stage.transform.position.y;

        stage.transform.position += swingPosCur;
    }

    void TurnStage()
    {
        if (isSwingingRight)  stage.transform.Rotate(Vector3.left, swingAngleCur, Space.World);
        else                  stage.transform.Rotate(-Vector3.left, swingAngleCur, Space.World);
    }

    void SpinStage()
    {
        spinAngleCur = options.GetCurrentStageValue().SpinSpeed * Time.fixedDeltaTime;

        if (isSpiningCW)  stage.transform.Rotate(Vector3.up, spinAngleCur, Space.Self);
        else              stage.transform.Rotate(-Vector3.up, spinAngleCur, Space.Self);
    }

    void ChangeDirection()
    {
        if (changeDir)
        {
            isSwingingRight = !isSwingingRight;
            changeDir = false;
        }
    }

    void SetStageValues()
    {
        stageVal.IsMachineSwinging = isMachineSwinging;
        stageVal.IsMachineSpining = isMachineSpining;
        stageVal.IsSpiningCW = isSpiningCW;

        stageVal.SwingPosCur = swingPosCur;
        stageVal.SpinAngleCur = spinAngleCur;
    }
    
}

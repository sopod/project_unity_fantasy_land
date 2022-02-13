using UnityEngine;


public class StageMovementValue
{
    public float Radius;
    public Vector3 SwingPosCur;
    public bool IsSwingRight;
    public float SwingAngleCur;
    public float SpinAngleCur;
}


public class Machine : MovingThing
{
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
    //float swingPowerMinPercent;

    Vector3 _swingPosCur;
    bool _changeDir;
    bool _isSwingingRight;
    float _swingAngleCur;
    float _swingAngleTotal;
    float _swingPowerCur;
    float _spinAngleCur;
    
    
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
        GameCenter.Instance.MoveCreaturesAlongMachine(isMachineSwinging, isMachineSpining, isSpiningCW);
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

        //swingPowerMinPercent = 0.3f; // min 30% will be same power 

        _changeDir = false;
        _isSwingingRight = true;
        _swingAngleCur = 0.0f;
        _swingAngleTotal = 0.0f;
        _swingPowerCur = 1.0f;

        PauseMoving();
    }

    public void ResetMachine()
    {
        stage.transform.position = stageStartPos;
        stage.transform.rotation = stageStartRot;
        swingBar.transform.position = barStartPos;
        swingBar.transform.rotation = barStartRot;

        _spinAngleCur = 0.0f;
        _changeDir = false;
        _isSwingingRight = true;
        _swingAngleCur = 0.0f;
        _swingAngleTotal = 0.0f;
        _swingPowerCur = 1.0f;
    }

    //void SetSwingPower()
    //{
    //    if (_swingAngleTotal <= swingAngleMax)
    //        _swingPowerCur = 1.0f - (_swingAngleTotal / swingAngleMax);
    //    else if (_swingAngleTotal >= swingAngleMax)
    //        _swingPowerCur = 1.0f - ((360.0f - _swingAngleTotal) / swingAngleMax);

    //    if (_swingPowerCur <= swingPowerMinPercent)
    //        _swingPowerCur = swingPowerMinPercent;
    //}

    void SetSwingAngleCur()
    {
        //SetSwingPower();

        _swingAngleCur = options.GetCurrentStageValue().SwingSpeed * Time.fixedDeltaTime * _swingPowerCur;

        if (_isSwingingRight)
            _swingAngleTotal += _swingAngleCur;
        else
            _swingAngleTotal -= _swingAngleCur;

        // _swingAngleTotal : 0 ~ max, 360 ~ 360-max
        if (_swingAngleTotal >= 360.0f)
            _swingAngleTotal -= 360.0f;
        else if (_swingAngleTotal <= 0.0f)
            _swingAngleTotal += 360.0f;

        // swingAngleMax <= _swingAngleTotal <= 360.0f - swingAngleMax
        if (options.GetCurrentStageValue().SwingAngleMax < _swingAngleTotal && _swingAngleTotal < (360.0f - options.GetCurrentStageValue().SwingAngleMax))
        {
            _changeDir = true;

            if (_swingAngleTotal <= 180.0f)
            {
                _swingAngleCur -= _swingAngleTotal - options.GetCurrentStageValue().SwingAngleMax;
                _swingAngleTotal = options.GetCurrentStageValue().SwingAngleMax;
            }
            else
            {
                _swingAngleCur -= (360.0f - options.GetCurrentStageValue().SwingAngleMax) - _swingAngleTotal;
                _swingAngleTotal = 360.0f - options.GetCurrentStageValue().SwingAngleMax;
            }
        }        
    }

    void SwingBar()
    {
        // forward - global
        if (_isSwingingRight)
            swingBar.transform.Rotate(Vector3.left, _swingAngleCur, Space.World);
        else
            swingBar.transform.Rotate(-Vector3.left, _swingAngleCur, Space.World);
    }

    void SwingStage()
    {
        float radian = Mathf.Deg2Rad * _swingAngleTotal;

        // left dir
        _swingPosCur.z = (Mathf.Sin(radian) * swingRadius) + stageStartPos.z - stage.transform.position.z;

        // up dir
        _swingPosCur.y = (swingRadius - Mathf.Cos(radian) * swingRadius) + stageStartPos.y - stage.transform.position.y;

        stage.transform.position += _swingPosCur;
    }

    void TurnStage()
    {
        // forward - global
        if (_isSwingingRight)
            stage.transform.Rotate(Vector3.left, _swingAngleCur, Space.World);
        else
            stage.transform.Rotate(-Vector3.left, _swingAngleCur, Space.World);
    }

    void SpinStage()
    {
        //_upDirBeforeSpin = stage.transform.up;

        _spinAngleCur = options.GetCurrentStageValue().SpinSpeed * Time.fixedDeltaTime;

        // up - local 
        if (isSpiningCW)
            stage.transform.Rotate(Vector3.up, _spinAngleCur, Space.Self);
        else
            stage.transform.Rotate(-Vector3.up, _spinAngleCur, Space.Self);
    }

    void ChangeDirection()
    {
        if (_changeDir)
        {
            _isSwingingRight = !_isSwingingRight;
            _changeDir = false;
        }
    }

    void SetStageValues()
    {
        stageVal.Radius = swingRadius;
        stageVal.SwingPosCur = _swingPosCur;
        stageVal.IsSwingRight = _isSwingingRight;
        stageVal.SwingAngleCur = _swingAngleCur;
        stageVal.SpinAngleCur = _spinAngleCur;
    }
    
}

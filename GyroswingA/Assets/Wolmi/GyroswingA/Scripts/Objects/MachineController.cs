using UnityEngine;


[System.Serializable]
public class StageMovementValue
{
    public float Radius;
    public Vector3 SwingPosCur;
    public bool IsSwingRight;
    public float SwingAngleCur;
    public float SpinAngleCur;
    //public Vector3 StageUpDir;
    //public Vector3 prevStagePos;
    //public float stageX;
}


public class MachineController : MovingThing
{
    StageMovementValue stageVal;
    Options options;

    [SerializeField] GameObject swingBar;
    [SerializeField] GameObject stage;
    
    Vector3 originPositionOfStage;
    float swingRadius;
    
    float swingPowerMinPercent;

    Vector3 _swingPosCur;
    bool _changeDir;
    bool _isSwingingRight;
    float _swingAngleCur;
    float _swingAngleTotal;
    float _swingPowerCur;
    float _spinAngleCur;
    
    
    void FixedUpdate()
    {
        if (!IsPaused())
        {
            //values.prevStagePos = stage.transform.position;

            SetSwingAngleCur();
            
            if (options.IsMachineSwinging)
            {
                SwingBar();
                SwingStage();
            }

            if (options.IsMachineTurning)
                TurnStage();

            if (options.IsMachineSpining)
                SpinStage();

            if (options.IsMachineSwinging)
                ChangeDirection();
            
            //values.stageX = stage.transform.rotation.eulerAngles.x;

            SetStageValues();
            GameManager.Instance.MoveCreaturesAlongStage();

            // if (IsStopped())  back to the original position
        }
    }

    public void SetMachine(Options options, StageMovementValue stageVal)
    {
        originPositionOfStage = stage.transform.position;
        swingRadius = (swingBar.transform.position.y - stage.transform.position.y);

        this.stageVal = stageVal;
        this.options = options;

        swingPowerMinPercent = 0.3f; // min 30% will be same power 
        //values.prevStagePos = stage.transform.position;

        _changeDir = false;
        _isSwingingRight = true;
        _swingAngleCur = 0.0f;
        _swingAngleTotal = 0.0f;
        _swingPowerCur = 1.0f;

        PauseMoving();
    }

    public void ResetMachine()
    {

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

        _swingAngleCur = options.GetCurLevelValues().SwingSpeedCur * Time.fixedDeltaTime * _swingPowerCur;

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
        if (options.GetCurLevelValues().SwingAngleMaxCur < _swingAngleTotal && _swingAngleTotal < (360.0f - options.GetCurLevelValues().SwingAngleMaxCur))
        {
            _changeDir = true;

            if (_swingAngleTotal <= 180.0f)
            {
                _swingAngleCur -= _swingAngleTotal - options.GetCurLevelValues().SwingAngleMaxCur;
                _swingAngleTotal = options.GetCurLevelValues().SwingAngleMaxCur;
            }
            else
            {
                _swingAngleCur -= (360.0f - options.GetCurLevelValues().SwingAngleMaxCur) - _swingAngleTotal;
                _swingAngleTotal = 360.0f - options.GetCurLevelValues().SwingAngleMaxCur;
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
        _swingPosCur.z = (Mathf.Sin(radian) * swingRadius) + originPositionOfStage.z - stage.transform.position.z;

        // up dir
        _swingPosCur.y = (swingRadius - Mathf.Cos(radian) * swingRadius) + originPositionOfStage.y - stage.transform.position.y;

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

        _spinAngleCur = options.GetCurLevelValues().SpinSpeedCur * Time.fixedDeltaTime;

        // up - local 
        if (options.IsSpiningCW)
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
        //stageVal.StageUpDir = ;
    }
    
}

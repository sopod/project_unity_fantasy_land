using UnityEngine;

public class MachineController : MovingThing
{
    [SerializeField] GameObject swingBar;
    [SerializeField] GameObject stage;
    
    Vector3 originPositionOfStage;
    float swingRadius;

    float swingSpeed;
    float swingAngleMax;
    float swingPowerMinPercent;
    float spinSpeed;
    bool isSpiningCW;

    bool isSwinging;
    bool isSpining;
    bool isTurning;

    bool _changeDir;
    bool _isSwingRight;
    float _swingAngleCur;
    float _swingAngleTotal;
    float _swingPowerCur;
    float _spinAngleCur;

    Vector3 _swingPosCur;
    Vector3 _upDirBeforeSpin;

    StageMovementValue values;
    
    public float Radius { get { return swingRadius; } }
    
    void FixedUpdate()
    {
        if (!IsPaused())
        {
            values.prevStagePos = stage.transform.position;

            SetSwingAngleCur();
            
            if (isSwinging)
            {
                SwingBar();
                SwingStage();
            }

            if (isTurning)
                TurnStage();

            if (isSpining)
                SpinStage();

            if (isSwinging)
                ChangeDirection();

            SetMoveValues();
            values.stageX = stage.transform.rotation.eulerAngles.x;
            GameManager.Instance.MoveCreaturesAlongStage(values);

            // if (IsStopped())  back to the original position
        }
    }

    public void SetMachine(Options options)
    {
        originPositionOfStage = stage.transform.position;
        swingRadius = (swingBar.transform.position.y - stage.transform.position.y);

        ChangeMachineMovement(options);

        ChangeMachineValues(options.GetCurLevelValues());
        this.isSpiningCW = values.isSpiningCW;

        swingPowerMinPercent = 0.3f; // min 30% will be same power 
        values.prevStagePos = stage.transform.position;

        _changeDir = false;
        _isSwingRight = true;
        _swingAngleCur = 0.0f;
        _swingAngleTotal = 0.0f;
        _swingPowerCur = 1.0f;



        PauseMoving();
    }

    public void ChangeMachineValues(LevelValues values)
    {
        this.swingSpeed = values.MachineSwingSpeed;
        this.swingAngleMax = values.MachineSwingAngleMax;
        this.spinSpeed = values.MachineSpinSpeed;
        //this.isSpiningCW = values.isSpiningCW;
    }

    public void ChangeMachineMovement(Options options)
    {
        this.isSpining = options.IsMachineSpining;
        this.isSwinging = options.IsMachineSwinging;
        this.isTurning = options.IsMachineTurning;
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

        _swingAngleCur = swingSpeed * Time.fixedDeltaTime * _swingPowerCur;

        if (_isSwingRight)
            _swingAngleTotal += _swingAngleCur;
        else
            _swingAngleTotal -= _swingAngleCur;

        // _swingAngleTotal : 0 ~ max, 360 ~ 360-max
        if (_swingAngleTotal >= 360.0f)
            _swingAngleTotal -= 360.0f;
        else if (_swingAngleTotal <= 0.0f)
            _swingAngleTotal += 360.0f;

        // swingAngleMax <= _swingAngleTotal <= 360.0f - swingAngleMax
        if (swingAngleMax < _swingAngleTotal && _swingAngleTotal < (360.0f - swingAngleMax))
        {
            _changeDir = true;

            if (_swingAngleTotal <= 180.0f)
            {
                _swingAngleCur -= _swingAngleTotal - swingAngleMax;
                _swingAngleTotal = swingAngleMax;
            }
            else
            {
                _swingAngleCur -= (360.0f - swingAngleMax) - _swingAngleTotal;
                _swingAngleTotal = 360.0f - swingAngleMax;
            }
        }        
    }

    void SwingBar()
    {
        // forward - global
        if (_isSwingRight)
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
        if (_isSwingRight)
            stage.transform.Rotate(Vector3.left, _swingAngleCur, Space.World);
        else
            stage.transform.Rotate(-Vector3.left, _swingAngleCur, Space.World);
    }

    void SpinStage()
    {
        _upDirBeforeSpin = stage.transform.up;

        _spinAngleCur = spinSpeed * Time.fixedDeltaTime;

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
            _isSwingRight = !_isSwingRight;
            _changeDir = false;
        }
    }

    void SetMoveValues()
    {
        values.swingPosCur = _swingPosCur;
        values.isSwingRight = _isSwingRight;
        values.swingAngleCur = _swingAngleCur;
        values.isSpiningCW = isSpiningCW;
        values.spinAngleCur = _spinAngleCur;
        values.stageUpDir = _upDirBeforeSpin;
        values.isSwinging = isSwinging;
        values.isTurning = isTurning;
        values.isSpining = isSpining;
    }
    
    void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(swingBar.transform.position, swingRadius);
    }
}

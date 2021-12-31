using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Lobby,
    InGame,
    End
}


public class GameManager : MonoBehaviour
{
    State state;

    [SerializeField] MachineController machine;
    [SerializeField] PlayerController player;
    [SerializeField] UIController ui;

    KeyManager keyManager;
    TimeManager timeManager;

    int _monsterCur = 0;
    
    // monster count
    int monsterMax = 9;

    // limit time
    int timeMax = 180;

    // machine
    bool isMachineSwinging = true;
    bool isMachineTurning = true;
    bool isMachineSpining = true;

    [Range(0, 50)] public
    float machineSwingSpeed = 10.0f;
    [Range(0, 90)] public
    float machineSwingAngleMax = 30.0f;
    [Range(0, 50)] public
    float machineSpinSpeed = 10.0f;
    bool isSpiningCW = true;

    // player
    [Range(0, 5)] public
    float playerMoveSpeed = 2.0f;
    [Range(0, 150)] public
    float playerRotateSpeed = 100.0f;
    [Range(0, 5)] public
    float playerJumpPower = 3.0f;
    


    public bool IsMachineStopped { get { return machine.IsStopped(); } }

    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;        
    }

    void Start()
    {
        InitInGame();
        StartInGame();
    }

    void Update()
    {
        UpdateInGame();
    }

    void InitInGame()
    {
        ChangeGameState(State.InGame);

        keyManager = new KeyManager();
        timeManager = new TimeManager();

        machine.InitMachine(machineSwingSpeed, machineSwingAngleMax, machineSpinSpeed, isSpiningCW, isMachineSwinging, isMachineTurning, isMachineSpining);
        player.InitPlayer(keyManager,playerMoveSpeed, playerRotateSpeed, playerJumpPower, machine.Radius, machineSpinSpeed, isSpiningCW);
        ui.InitUI(timeMax, monsterMax);
    }

    void StartInGame()
    {
        timeManager.StartTimer();
        machine.StartMoving();
        player.StartMoving();
    }

    void UpdateInGame()
    {
        if (state == State.InGame)
        {
            ui.UpdateTime(timeManager.GetCurrentTime());
        }
    }

    public void ChangeMachineValues()
    {
        machine.ChangeMachineValue(machineSwingSpeed, machineSwingAngleMax, machineSpinSpeed);
        machine.ChangeSpinDirection(isSpiningCW);

        player.ChangeMachineValue(machineSpinSpeed);
        player.ChangeSpinDirection(isSpiningCW);
    }

    public void SetGameOver()
    {
        ChangeGameState(State.End);
        player.StopMoving();
        machine.StopMoving();
    }

    public void SetWin()
    {
        ChangeGameState(State.End);
        player.PauseMoving();
        machine.PauseMoving();
    }

    public void SetPause()
    {
        player.PauseMoving();
        machine.PauseMoving();
    }

    public void KilledMonster()
    {
        _monsterCur--;
        ui.UpdateMonsterCount(_monsterCur);

        if (_monsterCur == 0)
            SetWin();
    }

    public void MovePlayerAlongStage(Vector3 swingPosCur, bool isSwingRight, float swingAngleCur, bool isSpiningCW, float spinAngleCur, Vector3 stageUpDir,
        bool isSwinging, bool isTurning, bool isSpining)
    {
        player.MovePlayerAlongStage(swingPosCur, isSwingRight, swingAngleCur, isSpiningCW, spinAngleCur, stageUpDir, isSwinging, isTurning, isSpining);
    }

    void ChangeGameState(State state)
    {
        this.state = state;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Lobby,
    InGame,
    End
}

public enum GameMode
{
    Easy, 
    Hard
}



public class GameManager : MonoBehaviour
{
    State state;
    GameMode mode;

    [SerializeField] MachineController machine;
    [SerializeField] PlayerController player;
    [SerializeField] UIController ui;
    [SerializeField] EnemySpawner enemySpawner;


    [SerializeField] GameObject stage;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject[] enemySpawnPositions;
    int enemyPrepareAmount = 5;

    KeyManager keyManager;
    TimeManager timeManager;

    int _remainingMonsterCur = 0;
    int _killedMonsterCur = 0;
    int _starCur = 0;

    int secondsMax { get { return 180; } }
    int starForHardMode { get { return 25; } }
    int stageAmountByMode { get { return 10; } }
    int monsterMax { get { return (mode == GameMode.Easy ? 5 : 9); } }
    

    // machine
    bool isMachineSwinging = true;
    bool isMachineTurning = true;
    bool isMachineSpining = true;

    //[Range(0, 50)] public
    float machineSwingSpeed = 10.0f;
    //[Range(0, 90)] public
    float machineSwingAngleMax = 30.0f;
    //[Range(0, 50)] public
    float machineSpinSpeed = 10.0f;
    bool isSpiningCW = true;

    // player
    //[Range(0, 5)] public
    float playerMoveSpeed = 2.0f;
    //[Range(0, 150)] public
    float playerRotateSpeed = 100.0f;
    //[Range(0, 5)] public
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
        SpawnEnemies();
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

        machine.SetMachine(machineSwingSpeed, machineSwingAngleMax, machineSpinSpeed, isSpiningCW, isMachineSwinging, isMachineTurning, isMachineSpining);
        player.SetPlayer(stage, keyManager, playerMoveSpeed, playerRotateSpeed, playerJumpPower, machine.Radius, machineSpinSpeed, isSpiningCW);
        
        ui.SetUI(secondsMax, monsterMax);

        enemySpawner.SetEnemySpawner(enemyPrefabs, enemySpawnPositions, enemyPrepareAmount);
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawnPositions.Length; i++)
        {
            GameObject e = enemySpawner.SpawnRandomEnemy();
            e.GetComponent<EnemyController>().SetCreature(stage, playerMoveSpeed, playerRotateSpeed, playerJumpPower, machine.Radius, machineSpinSpeed, isSpiningCW);
        }
    }
    void StartInGame()
    {
        timeManager.StartTimer();
        machine.StartMoving();
        player.StartMoving();

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            enemySpawner.spawnedEnemies[i].GetComponent<EnemyController>().StartMoving();
        }
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
        _remainingMonsterCur--;
        ui.UpdateMonsterCount(_remainingMonsterCur);

        if (_remainingMonsterCur == 0)
            SetWin();
    }

    int GetStarAmountForCurStage()
    {
        switch (mode)
        {
            case GameMode.Easy:
            {
                if (_remainingMonsterCur <= 1)
                    return 3;
                else if (_remainingMonsterCur <= 3)
                    return 2;
                else if (_remainingMonsterCur <= 4)
                    return 1;
                else
                    return 0;
            }

            case GameMode.Hard:
            {
                if (_remainingMonsterCur <= 1)
                    return 3;
                else if (_remainingMonsterCur <= 5)
                    return 2;
                else if (_remainingMonsterCur <= 7)
                    return 1;
                else
                    return 0;
            }
        }

        return 0;
    }

    public void MoveCreaturesAlongStage(Vector3 swingPosCur, bool isSwingRight, float swingAngleCur, bool isSpiningCW,
        float spinAngleCur, Vector3 stageUpDir,
        bool isSwinging, bool isTurning, bool isSpining)
    {
        player.MoveAlongStage(swingPosCur, isSwingRight, swingAngleCur, isSpiningCW, spinAngleCur, stageUpDir,
            isSwinging, isTurning, isSpining);

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            enemySpawner.spawnedEnemies[i].GetComponent<EnemyController>().MoveAlongStage(swingPosCur, isSwingRight, swingAngleCur, isSpiningCW, spinAngleCur, stageUpDir,
                isSwinging, isTurning, isSpining);
        }
    }
    void ChangeGameState(State state)
    {
        this.state = state;
    }

}
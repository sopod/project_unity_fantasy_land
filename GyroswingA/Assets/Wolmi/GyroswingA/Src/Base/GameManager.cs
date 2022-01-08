using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public enum GameState
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

public struct StageMovementValue
{
    public Vector3 swingPosCur;
    public bool isSwingRight;
    public float swingAngleCur;
    public bool isSpiningCW;
    public float spinAngleCur;
    public Vector3 stageUpDir;
    public bool isSwinging;
    public bool isTurning;
    public bool isSpining;
    public Vector3 prevStagePos;
    public float stageX;
}



public class GameManager : MonoBehaviour
{
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public LayerMask stageLayer;
    public LayerMask failZoneLayer;
    public LayerMask platformLayer;
    public LayerMask stagePoleLayer;

    GameState _gameState;
    GameMode mode;

    [SerializeField] MachineController machine;
    [SerializeField] PlayerController player;
    [SerializeField] UIController ui;
    [SerializeField] EnemySpawner enemySpawner;

    [SerializeField] GameObject stage;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject[] enemySpawnPositions;

    KeyController _keyController;
    TimeController _timeController;

    Options options;

    int _remainingMonsterCur = 0;
    int _killedMonsterCur = 0;
    int _starCur = 0;

    int enemyPrepareAmount { get { return 5; } }
    int secondsMax { get { return 180; } }
    int starForHardMode { get { return 25; } }
    int stageAmountByMode { get { return 10; } }
    int monsterMax { get { return (mode == GameMode.Easy ? 5 : 9); } }

    public bool IsMachineStopped { get { return machine.IsStopped(); } }

    public Vector3 PlayerPosition { get { return player.CenterPosition; } }


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

        options = GetComponent<Options>();
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
        ChangeGameState(GameState.InGame);

        _keyController = new KeyController();
        _timeController = new TimeController();

        machine.SetMachine(options);
        player.SetPlayer(stage, _keyController, machine.Radius, options);
        
        ui.SetUI(secondsMax, monsterMax);

        enemySpawner.SetEnemySpawner(enemyPrefabs, enemySpawnPositions, enemyPrepareAmount);
    }

    void SpawnEnemies()
    {
        //for (int i = 0; i < enemySpawnPositions.Length; i++)
        {
            GameObject e = enemySpawner.SpawnRandomEnemy();
            e.GetComponent<EnemyController>().SetEnemy(stage, player.gameObject, machine.Radius, options);
        }
    }
    void StartInGame()
    {
        _timeController.StartTimer();
        machine.StartMoving();
        player.StartMoving();

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            enemySpawner.spawnedEnemies[i].GetComponent<EnemyController>().StartMoving();
        }
    }

    void UpdateInGame()
    {
        if (_gameState == GameState.InGame)
        {
            ui.UpdateTime(_timeController.GetCurrentTime());
        }
    }

    public void ChangeMachineValues()
    {
        machine.ChangeMachineValue(options.machineSwingSpeed, options.machineSwingAngleMax, options.machineSpinSpeed);
        machine.ChangeSpinDirection(options.isSpiningCW);

        player.ChangeMachineValue(options.machineSpinSpeed);
        player.ChangeSpinDirection(options.isSpiningCW);
    }

    public void SetGameOver()
    {
        ChangeGameState(GameState.End);
        player.StopMoving();
        machine.StopMoving();
    }

    public void SetWin()
    {
        ChangeGameState(GameState.End);
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

    public void MoveCreaturesAlongStage(StageMovementValue values)
    {
        player.MoveAlongStage(values);

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            enemySpawner.spawnedEnemies[i].GetComponent<EnemyController>().MoveAlongStage(values);
        }
        
    }

    public Vector3 GetEnemyPosition(int enemySN)
    {
        return enemySpawner.spawnedEnemies[enemySN].transform.position;
    }

    void ChangeGameState(GameState gameState)
    {
        this._gameState = gameState;
    }



}
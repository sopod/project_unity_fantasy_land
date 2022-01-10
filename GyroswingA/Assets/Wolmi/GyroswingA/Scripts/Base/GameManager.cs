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
    public LayerMask stageBoundaryLayer;

    [SerializeField] MachineController machine;
    [SerializeField] PlayerController player;
    [SerializeField] InGameUIController _inGameUi;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] GameObject stage;
    [SerializeField] UISoundPlayer uiSoundPlayer;
    [SerializeField] Options options;

    KeyController keyController;
    TimeController gameTimer;

    GameState gameState;
    GameMode gameMode;

    bool isSceneSet;

    int _levelCur = 0;
    int _remainingMonsterCur = 0;
    int _killedMonsterCur = 0;
    int _starCur = 0;



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

        isSceneSet = false;
    }

    void Start()
    {
        //InitInGame();
        //SpawnEnemies();
        //StartInGame();
    }

    void Update()
    {
        WaitForTheFirstUpdate();

        if (isSceneSet)
        {
            UpdateInGame();
        }
    }

    void WaitForTheFirstUpdate()
    {
        if (!isSceneSet)
        {
            isSceneSet = true;
            ChangeSceneToInGame();
        }
    }

    public void ChangeSceneToInGame()
    {
        InitInGame();
        SpawnEnemies();

        ChangeGameState(GameState.InGame);
        StartInGame();
    }

    void InitInGame()
    {
        keyController = new KeyController();
        gameTimer = new TimeController();

        machine.SetMachine(options);
        player.SetPlayer(stage, keyController, machine.Radius, options);
        
        _inGameUi.SetUI(options.LimitSecondsPerStage, options.GetMonsterAmountForCurState(gameMode));

        enemySpawner.SetEnemySpawner(options.EnemyPrepareAmount);
    }

    void SpawnEnemies()
    {
        //for (int i = 0; i < enemySpawnPositions.Length; i++)
        {
            GameObject e = enemySpawner.SpawnRandomEnemy(options.GetCurLevelValues().EnemyTypes);
            e.GetComponent<EnemyController>().SetEnemy(stage, player.gameObject, machine.Radius, options);
        }
    }
    void StartInGame()
    {
        gameTimer.StartTimer();
        machine.StartMoving();
        player.StartMoving();

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            enemySpawner.spawnedEnemies[i].GetComponent<EnemyController>().StartMoving();
        }

        uiSoundPlayer.PlayBGM(gameState);
    }

    void UpdateInGame()
    {
        if (gameState == GameState.InGame)
        {
            _inGameUi.UpdateTime(gameTimer.GetCurrentTime());
        }
    }


    public void ChangeLevel()
    {
        _levelCur++;
        options.SetLevel(_levelCur);
        ChangeLevelValues();
    }

    void ChangeLevelValues()
    {
        machine.ChangeMachineValues(options.GetCurLevelValues());
        player.ChangeMachineValues(options.GetCurLevelValues());


        // enemy

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
        _inGameUi.UpdateMonsterCount(_remainingMonsterCur);

        if (_remainingMonsterCur == 0)
            SetWin();
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
        this.gameState = gameState;
    }



}
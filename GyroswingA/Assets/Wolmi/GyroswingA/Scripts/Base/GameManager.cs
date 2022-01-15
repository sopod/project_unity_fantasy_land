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



public class GameManager : MonoBehaviour
{
    [SerializeField] MachineController machine;
    [SerializeField] PlayerController player;
    [SerializeField] InGameUIController inGameUi;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] GameObject stage;
    [SerializeField] UISoundPlayer uiSoundPlayer;

    [SerializeField] StageMovementValue stageVal;
    [SerializeField] Options options;
    [SerializeField] StarCollector starCollector;

    TimeController gameTimer;

    GameState _gameStateCur;
    GameMode _gameModeCur;

    bool _isSceneSet;
    bool _isGamePaused;

    int _levelCur = 0;
    int _monsterMaxCur = 0;
    int _remainingMonsterCur = 0;


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

        _isSceneSet = false;
        _isGamePaused = true;
    }

    //void Start()
    //{
        //SetInGame();
        //SpawnEnemies();
        //StartInGame();
    //}

    void WaitForTheFirstUpdate()
    {
        if (!_isSceneSet)
        {
            _isSceneSet = true;

            UpgradeLevel();
            
            SetInGame();

            PrepareInGame();

            StartInGame();
        }
    }
    void SetInGame()
    {
        gameTimer = new TimeController();
        starCollector = new StarCollector();
        stageVal = new StageMovementValue();

        machine.SetMachine(options, stageVal);
        player.SetPlayer(stage, stageVal, options);

        enemySpawner.SetEnemySpawner(options.EnemyPrepareAmount);
    }

    void Update()
    {
        WaitForTheFirstUpdate();

        if (_isSceneSet && !_isGamePaused)
        {
            UpdateInGame();
        }
    }

    void PrepareInGame() // use this after upgrade level
    {
        SetPause();
        SpawnEnemies();

        _monsterMaxCur = options.GetMonsterAmountForCurState();
        inGameUi.SetUI(options.LimitSecondsPerStage, _monsterMaxCur);
        _remainingMonsterCur = _monsterMaxCur;

        player.ResetCreature(options.GetCurLevelValues());
        player.transform.position = options.PlayerStartPos;

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            EnemyController e = enemySpawner.spawnedEnemies[i].GetComponent<EnemyController>();
            e.ResetCreature(options.GetCurLevelValues());
        }

        ChangeGameState(GameState.InGame);
    }

    void StartInGame()
    {
        uiSoundPlayer.PlayBGM(_gameStateCur);

        _isGamePaused = false;

        gameTimer.StartTimer(options.LimitSecondsPerStage);
        machine.StartMoving();
        player.StartMoving();

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            enemySpawner.spawnedEnemies[i].GetComponent<EnemyController>().StartMoving();
        }
    }

    void UpdateInGame()
    {
        if (_gameStateCur == GameState.InGame)
        {
            inGameUi.UpdateTime(gameTimer.GetCurrentTime());

            if (gameTimer.IsFinished)
            {
                EndCurLevel();
            }
        }
    }
    void SpawnEnemies()
    {
        //for (int i = 0; i < _monsterMaxCur; i++)
        {
            GameObject e = enemySpawner.SpawnRandomEnemy(options.GetCurLevelValues().EnemyTypesCur);
            e.GetComponent<EnemyController>().SetEnemy(stage, stageVal, options);
        }
    }

    public void UpgradeLevel()
    {
        _levelCur++;
        options.ChangeLevel(_gameModeCur, _levelCur);
    }

    public void SetWin()
    {
        ChangeGameState(GameState.End);
        player.PauseMoving();
        //machine.PauseMoving();

        // win UI here
    }

    public void SetGameOver()
    {
        ChangeGameState(GameState.End);
        player.StopMoving();
        //machine.StopMoving();

        // lose UI here
    }

    public void SetPause()
    {
        player.PauseMoving();
        machine.PauseMoving();

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            enemySpawner.spawnedEnemies[i].GetComponent<EnemyController>().PauseMoving();
        }
    }

    void EndCurLevel()
    {
        ChangeGameState(GameState.End);
        float star = starCollector.GetStarForCurStage(_gameModeCur, _remainingMonsterCur);

        if (star == 0)
            SetGameOver();
        else
            SetWin();
    }
 
    public void OnMonsterKilled()
    {
        _remainingMonsterCur--;
        inGameUi.UpdateMonsterCount(_remainingMonsterCur);

        if (_remainingMonsterCur == 0)
        {
            EndCurLevel();
            enemySpawner.ReturnAllEnemy();
        }
    }

    public void MoveCreaturesAlongStage()
    {
        player.MoveAlongStage();

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            enemySpawner.spawnedEnemies[i].GetComponent<EnemyController>().MoveAlongStage();
        }
    }
    
    void ChangeGameState(GameState gameState)
    {
        this._gameStateCur = gameState;
    }



}
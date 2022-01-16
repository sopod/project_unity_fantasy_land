using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public enum GameState
{
    Lobby,
    InGame,
    Pause,
    Result
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
    [SerializeField] ObjectSpawner enemySpawner;
    [SerializeField] ObjectSpawner itemSpawner;
    [SerializeField] GameObject stage;
    [SerializeField] UISoundPlayer uiSoundPlayer;

    [SerializeField] StageData stageData;
    [SerializeField] StageMovementValue stageVal;
    [SerializeField] Options options;
    [SerializeField] StarCollector starCollector;

    TimeController gameTimer;

    GameState _gameStateCur;
    [SerializeField] GameMode _gameModeCur;
    [SerializeField] int _levelCur = 0;

    bool _isSceneSet;
    bool _isGamePaused;

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
    

    void WaitForTheFirstUpdate()
    {
        if (!_isSceneSet)
        {
            _isSceneSet = true;

            _levelCur = stageData.stageNumberCur;
            _gameModeCur = stageData.stageModeCur;
            options.ChangeLevel(_gameModeCur, _levelCur);

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

        enemySpawner.SetSpawner(options.EnemyPrepareAmount);
    }

    void Update()
    {
        WaitForTheFirstUpdate();

        if (_isSceneSet)
        {
            UpdateInGame();
        }
    }



    void PrepareInGame() // use this after upgrade level
    {
        machine.ResetMachine();

        player.ResetCreature();
        player.transform.position = options.PlayerStartPos;
        player.transform.rotation = options.PlayerStartRot;
        
        SpawnEnemies();

        SetPauseMoving();

        if(!uiSoundPlayer.IsBGMPlaying)
            uiSoundPlayer.PlayBGM(GameState.InGame);
    }

    void StartInGame()
    {
        ChangeGameState(GameState.InGame);

        _monsterMaxCur = options.GetMonsterAmountForCurState();
        inGameUi.SetUI(options.LimitSecondsPerStage, _monsterMaxCur);
        _remainingMonsterCur = _monsterMaxCur;

        gameTimer.StartTimer(options.LimitSecondsPerStage);

        SetStartMoving();
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
        EnemyType[] typesToGen = options.GetCurLevelValues().EnemyTypesCur;
        int amount = options.GetMonsterAmountForCurState();

        for (int i = 0; i < amount; i++)
        {
            GameObject e = enemySpawner.SpawnEnemyObject(typesToGen, amount);
            e.GetComponent<EnemyController>().SetEnemy(stage, stageVal, options);
        }
    }
    
    public bool UpgradeLevel()
    {
        _levelCur++;

        if (_levelCur > stageData.StageCountPerMode && _gameModeCur == GameMode.Easy)
        {
            _levelCur = 1;
            _gameModeCur = GameMode.Hard;

        }
        else if (_levelCur > stageData.StageCountPerMode && _gameModeCur == GameMode.Hard)
        {
            return false;
        }

        options.ChangeLevel(_gameModeCur, _levelCur);
        stageData.SetUnlocked(_gameModeCur, _levelCur);

        return true;
    }

    void EndCurLevel()
    {
        ChangeGameState(GameState.Result);

        int star = starCollector.GetStarForCurStage(_gameModeCur, _remainingMonsterCur);

        stageData.SetStar(_gameModeCur, _levelCur, star);

        enemySpawner.ReturnAllObjects();
        itemSpawner.ReturnAllObjects();

        if (star == 0)
        {
            SetFail();
        }
        else
        {
            SetWin();
        }
    }

    public void SetWin()
    {
        SetPauseMoving();
        ChangeGameState(GameState.Result);
        
        inGameUi.SetWinUI(starCollector.GetStarCur);

        // call restart level
        if (!UpgradeLevel())
        {
            Invoke("BackToStageSelectionScene", options.ResultUIShowingTime);
        }
        else
        {
            Invoke("PrepareInGame", options.ResultUIShowingTime);
            Invoke("TurnResultUIOff", options.ResultUIShowingTime);
            Invoke("StartInGame", options.GameStartWaitingTime);
        }
    }

    public void SetFail()
    {
        SetPauseMoving();
        ChangeGameState(GameState.Result);

        inGameUi.SetLoseUI();

        // call return to stage selection scene function
        Invoke("BackToStageSelectionScene", options.ResultUIShowingTime);
    }

    public void SetStartMoving()
    {
        ChangeGameState(GameState.InGame);

        gameTimer.RestartTimer();

        machine.StartMoving();
        player.StartMoving();

        for (int i = 0; i < enemySpawner.spawnedObjects.Count; i++)
        {
            enemySpawner.spawnedObjects[i].GetComponent<EnemyController>().StartMoving();
        }
    }

    public void SetPauseMoving()
    {
        ChangeGameState(GameState.Pause);

        gameTimer.PauseTimer();

        player.PauseMoving();
        machine.PauseMoving();

        for (int i = 0; i < enemySpawner.spawnedObjects.Count; i++)
        {
            enemySpawner.spawnedObjects[i].GetComponent<EnemyController>().PauseMoving();
        }
    }

    public void OnMonsterKilled()
    {
        _remainingMonsterCur--;
        inGameUi.UpdateMonsterCount(_remainingMonsterCur);

        if (_remainingMonsterCur == 0)
        {
            EndCurLevel();
        }
    }

    public void MoveCreaturesAlongStage()
    {
        player.MoveAlongStage();

        for (int i = 0; i < enemySpawner.spawnedObjects.Count; i++)
        {
            enemySpawner.spawnedObjects[i].GetComponent<EnemyController>().MoveAlongStage();
        }
    }
    
    void ChangeGameState(GameState gameState)
    {
        this._gameStateCur = gameState;
    }

    void BackToStageSelectionScene()
    {
        SceneLoader.LoadScene("StageSelection");
    }

    void TurnResultUIOff()
    {
        inGameUi.TurnOffResultUI();
    }

}
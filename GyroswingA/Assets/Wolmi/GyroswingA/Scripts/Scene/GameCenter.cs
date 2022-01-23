using UnityEngine;

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


public class GameCenter : MonoBehaviour
{
    [Header("------- Attached components")]
    [SerializeField] Machine machine;
    [SerializeField] Player player;
    [SerializeField] InGameUIDisplay inGameUi;
    [SerializeField] ObjectSpawner enemySpawner;
    [SerializeField] ObjectSpawner itemSpawner;
    [SerializeField] GameObject stage;
    [SerializeField] UISoundPlayer uiSoundPlayer;


    [Header("------- Game Status")]
    [SerializeField] GameMode _gameModeCur;
    [SerializeField] int _levelCur = 0;
    GameState _gameStateCur;


    [Header("------- Game Data")]
    [SerializeField] StarDataPerLevel levelDataPerLevel;
    [SerializeField] Options options;
    StageMovementValue stageVal;

    StarCollector starCollector;
    StopWatch gameTimer;


    bool _isSceneSet;

    int _monsterMaxCur = 0;
    int _remainingMonsterCur = 0;


    public bool IsMachineStopped { get { return machine.IsStopped(); } }
    public Vector3 PlayerPosition { get { return player.CenterPosition; } }


    static GameCenter instance;
    public static GameCenter Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameCenter>();
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;

        _isSceneSet = false;
    }
    

    void WaitForTheFirstUpdate()
    {
        if (!_isSceneSet)
        {
            inGameUi.gameObject.SetActive(false);

            _isSceneSet = true;

            _levelCur = levelDataPerLevel.levelNumberCur;
            _gameModeCur = levelDataPerLevel.stageModeCur;
            options.ChangeLevel(_gameModeCur, _levelCur);

            options.ResetOptionValuesByCode();

            SetInGame();

            PrepareInGame();

            SetStopMoving();
            machine.StartMoving();

            Invoke("StartInGame", options.WaitingTimeForCinemachine);
        }
    }

    void SetInGame()
    {
        gameTimer = new StopWatch();
        starCollector = new StarCollector();
        stageVal = new StageMovementValue();

        machine.SetMachine(options, stageVal);
        player.SetPlayer(stage, stageVal, options);

        enemySpawner.SetSpawner(options);
        itemSpawner.SetSpawner(options);
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
        options.ResetOptions();

        machine.ResetMachine();

        player.ResetCreature();
        player.transform.position = options.PlayerStartPos;
        player.transform.rotation = options.PlayerStartRot;
        
        SpawnEnemies();
        SpawnItems();

        SetPauseMoving();

        uiSoundPlayer.PlayBGM(GameState.InGame);
    }

    void StartInGame()
    {
        ChangeGameState(GameState.InGame);
        
        inGameUi.gameObject.SetActive(true);

        _monsterMaxCur = options.GetMonsterAmountForCurState();
        inGameUi.SetUI(options.LimitSecondsPerStage, _monsterMaxCur);
        _remainingMonsterCur = _monsterMaxCur;

        gameTimer.StartTimer(options.LimitSecondsPerStage);

        SetStartMoving(true);
    }

    void UpdateInGame()
    {
        if (_gameStateCur == GameState.InGame)
        {
            inGameUi.UpdateTime(gameTimer.GetRemainingTime());

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
            e.GetComponent<Enemy>().SetEnemy(stage, stageVal, options);
        }
    }

    void SpawnItems()
    {
        ItemType[] typesToGen = options.GetCurLevelValues().ItemTypesCur;

        for (int i = 0; i < typesToGen.Length; i++)
        {
            GameObject e = itemSpawner.SpawnItemObject(typesToGen[i]);
            e.GetComponent<Item>().SetItem(options);
        }
    }

    public bool UpgradeLevel() // after get star
    {
        // calculate next level and mode
        _levelCur++;

        if (_levelCur > levelDataPerLevel.LevelCountPerMode && _gameModeCur == GameMode.Easy)
        {
            if (!levelDataPerLevel.IsHardModeOpen)
            {
                return false;
            }

            _levelCur = 1;
            _gameModeCur = GameMode.Hard;
        }
        else if (_levelCur > levelDataPerLevel.LevelCountPerMode && _gameModeCur == GameMode.Hard)
        {
            return false;
        }
        

        // save data
        options.ChangeLevel(_gameModeCur, _levelCur);
        levelDataPerLevel.SetUnlocked(_gameModeCur, _levelCur);

        GameDataLoader.SaveStarDataFile(levelDataPerLevel);

        return true;
    }

    void EndCurLevel()
    {
        ChangeGameState(GameState.Result);

        int star = starCollector.GetStarForCurStage(_gameModeCur, _remainingMonsterCur);

        levelDataPerLevel.SetStar(_gameModeCur, _levelCur, star);

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
        
        uiSoundPlayer.StopPlayingBGM();
        Invoke("SetWinBGM", options.ResultSoundWaitingTime);
        Invoke("SetWinUI", options.ResultUIWaitingTime);

        // call restart level
        if (!UpgradeLevel())
        {
            Invoke("BackToStageSelectionScene", options.ResultUIRemainingTime);
        }
        else
        {
            Invoke("PrepareInGame", options.ResultUIRemainingTime);
            Invoke("TurnResultUIOff", options.ResultUIRemainingTime);
            Invoke("StartInGame", options.GameStartWaitingTime);
        }
    }

    public void SetFail()
    {
        SetPauseMoving();
        ChangeGameState(GameState.Result);
        
        uiSoundPlayer.StopPlayingBGM();
        Invoke("SetFailBGM", options.ResultSoundWaitingTime);
        Invoke("SetFailUI", options.ResultUIWaitingTime);


        // call return to stage selection scene function
        Invoke("BackToStageSelectionScene", options.ResultUIRemainingTime);
    }

    void SetWinBGM()
    {
        uiSoundPlayer.PlayResultBGM(true);
    }

    void SetFailBGM()
    {
        uiSoundPlayer.PlayResultBGM(false);
    }

    void SetWinUI()
    {
        inGameUi.SetWinUI(starCollector.GetStarCur);
    }

    void SetFailUI()
    {
        inGameUi.SetLoseUI();
    }

    public void SetStartMoving(bool waitEnemy = false)
    {
        ChangeGameState(GameState.InGame);

        gameTimer.RestartTimer();

        machine.StartMoving();
        player.StartMoving();

        if (waitEnemy)
        {
            for (int i = 0; i < enemySpawner.spawnedObjects.Count; i++)
            {
                enemySpawner.spawnedObjects[i].GetComponent<Enemy>().StopMoving();
            }
            Invoke("StartEnemyMove", options.EnemyStartWaitingTime);
        }
        else
        {
            StartEnemyMove();
        }

        for (int i = 0; i < itemSpawner.spawnedObjects.Count; i++)
        {
            itemSpawner.spawnedObjects[i].GetComponent<Item>().StartMoving();
        }
    }

    void StartEnemyMove()
    {
        for (int i = 0; i < enemySpawner.spawnedObjects.Count; i++)
        {
            enemySpawner.spawnedObjects[i].GetComponent<Enemy>().StartMoving();
        }
    }
    public void SetStopMoving()
    {
        gameTimer.PauseTimer();

        player.StopMoving();
        machine.StopMoving();

        for (int i = 0; i < enemySpawner.spawnedObjects.Count; i++)
        {
            enemySpawner.spawnedObjects[i].GetComponent<Enemy>().StopMoving();
        }

        for (int i = 0; i < itemSpawner.spawnedObjects.Count; i++)
        {
            itemSpawner.spawnedObjects[i].GetComponent<Item>().StopMoving();
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
            enemySpawner.spawnedObjects[i].GetComponent<Enemy>().PauseMoving();
        }

        for (int i = 0; i < itemSpawner.spawnedObjects.Count; i++)
        {
            itemSpawner.spawnedObjects[i].GetComponent<Item>().PauseMoving();
        }
    }

    public void OnTimeItemUsed(float plusTime)
    {
        gameTimer.ExtendTimer(plusTime);
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
        player.MoveAlongWithStage();

        for (int i = 0; i < enemySpawner.spawnedObjects.Count; i++)
        {
            enemySpawner.spawnedObjects[i].GetComponent<Enemy>().MoveAlongWithStage();
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
        inGameUi.SetGameUI();
    }
    
}
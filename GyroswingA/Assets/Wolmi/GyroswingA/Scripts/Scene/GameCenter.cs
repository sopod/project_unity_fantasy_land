using UnityEngine;

public enum GameState
{
    Playing,
    Pause,
    Result
}

public enum GameMode
{
    Easy, 
    Hard
}

[System.Serializable]
public struct Layers
{
    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;
    public LayerMask ItemLayer;
    public LayerMask StageLayer;
    public LayerMask FailZoneLayer;
    public LayerMask StageBoundaryLayer;
    public LayerMask ShootProjectileLayer;
}

public class GameCenter : MonoBehaviour
{
    [Header("------- Obejcts")]
    [SerializeField] Machine machine;
    [SerializeField] Player player;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] ItemSpawner itemSpawner;
    [SerializeField] ProjectileSpawner pjSpawner;
    [SerializeField] GameObject stage;
    [SerializeField] InGameUIDisplay inGameUi;
    [SerializeField] UISoundPlayer uiSoundPlayer;
    
    [Header("---- layers")]
    [SerializeField] Layers layerStruct;

    LevelChanger levelControl = new LevelChanger();
    StarCollector starCollector = new StarCollector();
    StageMovementValue stageVal = new StageMovementValue();
    StopWatch gameTimer = new StopWatch();

    const float waitingTimeForCinemachine = 8.0f;
    const int limitSecondsPerStage = 180;
    const float resultSoundWaitingTime = 0.3f;
    const float resultUIWaitingTime = 0.4f;
    const float resultUIRemainingTime = 5.0f;
    const float gameStartWaitingTime = 5.5f;
    const float enemyStartWaitingTime = 1.0f;

    GameState _gameStateCur;
    bool _isSceneSet;
    int _monsterMaxCur = 0;
    int _remainingMonsterCur = 0;

    public Vector3 PlayerPosition { get => player.CenterPosition; }

    static GameCenter instance;
    public static GameCenter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameCenter>();
            }
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
        if (_isSceneSet) return;

        _isSceneSet = true;
        
        uiSoundPlayer = UISoundPlayer.Instance;

        inGameUi.gameObject.SetActive(false);

        levelControl.Init(SceneController.Instance.loaderGoogleSheet, SceneController.Instance.loaderStarData);

        SetInGame();

        PrepareInGame();

        SetStopMoving();
        machine.StartMoving();

        Invoke("StartInGame", waitingTimeForCinemachine);
    }

    void SetInGame()
    {
        machine.SetMachine(levelControl, stageVal);
        player.SetPlayer(stage, stageVal, levelControl, layerStruct, pjSpawner);
    }

    void Update()
    {
        WaitForTheFirstUpdate();

        if (_isSceneSet)
            UpdateInGame();
    }

    void PrepareInGame() // use this after upgrade level
    {
        machine.ResetMachine();

        player.ResetCreature();
        
        SpawnEnemies();
        SpawnItems();

        SetPauseMoving();

        uiSoundPlayer.PlayBGM(SceneState.InGame);
    }

    void StartInGame()
    {
        ChangeGameState(GameState.Playing);
        
        inGameUi.gameObject.SetActive(true);

        _monsterMaxCur = levelControl.GetMonsterAmountForCurState();
        inGameUi.SetUI(limitSecondsPerStage, _monsterMaxCur);
        _remainingMonsterCur = _monsterMaxCur;

        gameTimer.StartTimer(limitSecondsPerStage);

        SetStartMoving(true);
    }

    void UpdateInGame()
    {
        if (_gameStateCur != GameState.Playing) return;

        inGameUi.UpdateTime(gameTimer.GetRemainingTime());

        if (gameTimer.IsFinished)
            EndCurLevel();
    }

    void SpawnEnemies()
    {
        EnemyType[] typesToGen = levelControl.GetCurLevelValues().EnemyTypes;
        int amount = levelControl.GetMonsterAmountForCurState();

        for (int i = 0; i < amount; i++)
        {
            GameObject e = enemySpawner.SpawnEnemyObject(typesToGen, amount);
            e.GetComponent<Enemy>().SetEnemy(stage, stageVal, levelControl, layerStruct, pjSpawner);
        }
    }

    void SpawnItems()
    {
        ItemType[] typesToGen = levelControl.GetCurLevelValues().ItemTypes;

        for (int i = 0; i < typesToGen.Length; i++)
        {
            GameObject e = itemSpawner.SpawnItemObject(typesToGen[i]);
        }
    }
    
    void EndCurLevel()
    {
        ChangeGameState(GameState.Result);

        enemySpawner.ReturnAllObjects();
        itemSpawner.ReturnAllObjects();

        bool isWin = starCollector.SetStar(SceneController.Instance.loaderStarData, levelControl.ModeCur, levelControl.LevelCur, _remainingMonsterCur);
        //int star = starCollector.GetStarForCurStage(levelControl.ModeCur, _remainingMonsterCur);
        //loaderStarData.data.SetStar(levelControl.ModeCur, levelControl.LevelCur, star);

        if (isWin)
            SetWin();
        else
            SetFail();
    }

    public void SetWin()
    {
        SetPauseMoving();
        ChangeGameState(GameState.Result);


        uiSoundPlayer.StopPlayingBGM();
        Invoke("SetWinBGM", resultSoundWaitingTime);
        Invoke("SetWinUI", resultUIWaitingTime);

        // call restart level
        if (!levelControl.UpgradeLevel())
        {
            Invoke("BackToStageSelectionScene", resultUIRemainingTime);
        }
        else
        {
            Invoke("PrepareInGame", resultUIRemainingTime);
            Invoke("TurnResultUIOff", resultUIRemainingTime);
            Invoke("StartInGame", gameStartWaitingTime);
        }
    }

    public void SetFail()
    {
        SetPauseMoving();
        ChangeGameState(GameState.Result);
        
        uiSoundPlayer.StopPlayingBGM();
        Invoke("SetFailBGM", resultSoundWaitingTime);
        Invoke("SetFailUI", resultUIWaitingTime);


        // call return to stage selection scene function
        Invoke("BackToStageSelectionScene", resultUIRemainingTime);
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
        ChangeGameState(GameState.Playing);

        gameTimer.RestartTimer();

        machine.StartMoving();
        player.StartMoving();

        if (waitEnemy)
        {
            for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
            {
                enemySpawner.spawnedEnemies[i].StopMoving();
            }
            Invoke("StartEnemyMove", enemyStartWaitingTime);
        }
        else
        {
            StartEnemyMove();
        }

        for (int i = 0; i < itemSpawner.spawnedItems.Count; i++)
        {
            itemSpawner.spawnedItems[i].StartMoving();
        }
    }

    void StartEnemyMove()
    {
        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            enemySpawner.spawnedEnemies[i].StartMoving();
        }
    }
    public void SetStopMoving()
    {
        gameTimer.PauseTimer();

        player.StopMoving();
        machine.StopMoving();

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            enemySpawner.spawnedEnemies[i].StopMoving();
        }

        for (int i = 0; i < itemSpawner.spawnedItems.Count; i++)
        {
            itemSpawner.spawnedItems[i].StopMoving();
        }
    }

    public void SetPauseMoving()
    {
        ChangeGameState(GameState.Pause);

        gameTimer.PauseTimer();

        player.PauseMoving();
        machine.PauseMoving();

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            enemySpawner.spawnedEnemies[i].PauseMoving();
        }

        for (int i = 0; i < itemSpawner.spawnedItems.Count; i++)
        {
            itemSpawner.spawnedItems[i].PauseMoving();
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
            EndCurLevel();
    }

    public void MoveCreaturesAlongStage(bool isMachineSwinging, bool isMachineSpining, bool isSpiningCW)
    {
        player.MoveAlongWithStage(isMachineSwinging, isMachineSpining, isSpiningCW);

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            enemySpawner.spawnedEnemies[i].MoveAlongWithStage(isMachineSwinging, isMachineSpining, isSpiningCW);
        }
    }
    
    void ChangeGameState(GameState gameState)
    {
        _gameStateCur = gameState;
    }

    void BackToStageSelectionScene()
    {
        SceneController.Instance.ChangeScene(SceneState.StageSelection);
    }

    void TurnResultUIOff()
    {
        inGameUi.SetGameUI();
    }
    
}
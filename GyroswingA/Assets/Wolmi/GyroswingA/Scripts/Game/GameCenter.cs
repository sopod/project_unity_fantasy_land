using System;
using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    ShowingResult
}

public enum GameMode
{
    Easy, 
    Hard
}


public class GameCenter : MonoBehaviour
{
    [Header("------- Obejcts")]
    [SerializeField] Player player;
    [SerializeField] Machine machine;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] ItemSpawner itemSpawner;
    [SerializeField] ProjectileSpawner projectileSpawner;

    [SerializeField] Transform playerCamera;
    [SerializeField] GameObject stageOfMachine;

    [Header("------- UI")]
    [SerializeField] InGameUIDisplay inGameUI;

    UISoundPlayer uiSoundPlayer { get => UISoundPlayer.Instance; }
    SceneController sceneController { get => SceneController.Instance; }

    [Header("------- layers")]
    [SerializeField] Layers layers;

    Schedule schedule = new Schedule();
    StageChanger stageChanger;
    StarCollector starCollector = new StarCollector();
    StageMovementValue stageVal = new StageMovementValue();
    StopWatch gameTimer = new StopWatch();

    const int LIMIT_TIME_PER_STAGE = 180;
    const float WAITING_TIME_TO_FINISH_TIMELINE = 8.0f;
    const float WAITING_TIME_TO_MAKE_ENEMY_START_MOVING = 1.0f;

    const float WAITING_TIME_TO_PLAY_RESULT_SOUND = 0.3f;
    const float WAITING_TIME_TO_PLAY_RESULT_UI = 0.4f;
    const float REMAINING_TIME_TO_SHOW_RESULT_UI = 5.0f;
    const float WAITING_TIME_TO_START_GAME = 5.5f;

    GameState gameStateCur;
    int monsterMaxCur = 0;
    int remainingMonstersCur = 0;

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateGame();
    }

    void Init()
    {
        stageChanger = new StageChanger(sceneController.loaderGoogleSheet, sceneController.loaderStarData);

        machine.Init(stageChanger, stageVal);
        player.InitPlayer(stageOfMachine, stageVal, stageChanger, layers, projectileSpawner, playerCamera);

        machine.OnMachineMoved = MoveCreaturesAlongMachine;
        player.OnDead = OnFail;
        player.OnItemGot = (time) => { OnItemGot(time); };

        MakeSchedule();

        PrepareGame();

        MakeObjectsStopMoving();
        machine.StartMoving();

        Invoke("StartGame", WAITING_TIME_TO_FINISH_TIMELINE);
    }

    
    void PrepareGame()
    {
        machine.ResetMachine();
        player.ResetValues();
        
        SpawnEnemies();
        SpawnItems();

        MakeObjectPaused();

        uiSoundPlayer.PlayBGM(BgmSoundType.InGame, true);
    }

    void StartGame()
    {
        ChangeGameState(GameState.Playing);
        
        monsterMaxCur = stageChanger.GetMonsterMaxForCurrentStage();
        inGameUI.SetGameStartUI(LIMIT_TIME_PER_STAGE, monsterMaxCur, stageChanger.StageCur);
        remainingMonstersCur = monsterMaxCur;

        gameTimer.StartTimer(LIMIT_TIME_PER_STAGE);

        MakeObjectsStartMoving(true);
    }

    void UpdateGame()
    {
        if (gameStateCur != GameState.Playing) return;

        inGameUI.UpdateTime(gameTimer.GetRemainingTime());

        if (gameTimer.IsFinished)
            EndCurrentStage();
    }

    void SpawnEnemies()
    {
        EnemyType[] typesToGen = stageChanger.GetCurrentStageValue().EnemyTypes;
        int amount = stageChanger.GetMonsterMaxForCurrentStage();

        for (int i = 0; i < amount; i++)
        {
            GameObject obj = enemySpawner.SpawnEnemyObject(typesToGen, amount);
            Enemy e = obj.GetComponent<Enemy>();
            e.InitEnemy(stageOfMachine, stageVal, stageChanger, layers, projectileSpawner, player.transform, playerCamera);
            e.OnDead = OnEnemyKilled;
        }
    }

    void SpawnItems()
    {
        ItemType[] typesToGen = stageChanger.GetCurrentStageValue().ItemTypes;

        for (int i = typesToGen.Length - 1; i >= 0; i--)
        {
            GameObject item = itemSpawner.SpawnItemObject(typesToGen[i]);
        }
    }
    
    void EndCurrentStage()
    {
        ChangeGameState(GameState.ShowingResult);


        bool isWin = starCollector.SetStar(sceneController.loaderStarData, stageChanger.ModeCur, stageChanger.StageCur, remainingMonstersCur);

        if (isWin) OnWin();
        else OnFail();
    }

    public void OnWin()
    {
        MakeObjectPaused();
        ChangeGameState(GameState.ShowingResult);

        uiSoundPlayer.StopPlayingBGM();
        
        if (!stageChanger.UpgradeStage())
        {
            schedule.onWinTotally?.Invoke();
            return;
        }

        schedule.onWin?.Invoke();
    }

    public void OnFail()
    {
        MakeObjectPaused();
        ChangeGameState(GameState.ShowingResult);
        
        uiSoundPlayer.StopPlayingBGM();        
        schedule.onFail?.Invoke();
    }

    public void MakeObjectsStartMoving(bool makeEnemyWait = false)
    {
        ChangeGameState(GameState.Playing);

        gameTimer.RestartTimer();

        machine.StartMoving();
        player.StartMoving();

        for (int i = itemSpawner.spawnedItems.Count - 1; i >= 0; i--)
            itemSpawner.spawnedItems[i].StartMoving();

        if (makeEnemyWait)
        {
            for (int i = enemySpawner.spawnedEnemies.Count - 1; i >= 0; i--)
                enemySpawner.spawnedEnemies[i].StopMoving();

            Invoke("MakeEnemyStartMoving", WAITING_TIME_TO_MAKE_ENEMY_START_MOVING);
            return;
        }

        MakeEnemyStartMoving();
    }

    void MakeEnemyStartMoving()
    {
        for (int i = enemySpawner.spawnedEnemies.Count - 1; i >= 0; i--)
            enemySpawner.spawnedEnemies[i].StartMoving();
    }

    public void MakeObjectsStopMoving()
    {
        gameTimer.PauseTimer();

        player.StopMoving();
        machine.StopMoving();

        for (int i = enemySpawner.spawnedEnemies.Count - 1; i >= 0; i--)
            enemySpawner.spawnedEnemies[i].StopMoving();

        for (int i = itemSpawner.spawnedItems.Count - 1; i >= 0; i--)
            itemSpawner.spawnedItems[i].StopMoving();
    }

    public void MakeObjectPaused()
    {
        ChangeGameState(GameState.Paused);

        gameTimer.PauseTimer();

        player.PauseMoving();
        machine.PauseMoving();

        for (int i = enemySpawner.spawnedEnemies.Count - 1; i >= 0; i--)
            enemySpawner.spawnedEnemies[i].PauseMoving();

        for (int i = itemSpawner.spawnedItems.Count - 1; i >= 0; i--)
            itemSpawner.spawnedItems[i].PauseMoving();
    }

    public void OnItemGot(float plusTime)
    {
        gameTimer.ExtendTimer(plusTime);
        inGameUI.NotifyItemText(plusTime > 0);
    }

    public void OnEnemyKilled()
    {
        remainingMonstersCur--;
        inGameUI.UpdateMonsterCount(remainingMonstersCur);

        if (remainingMonstersCur == 0)
        {
            EndCurrentStage();
            return;
        }

        uiSoundPlayer.PlayUISound(UIEffectSoundType.KilledEnemy);
    }

    public void MoveCreaturesAlongMachine()
    {
        player.MoveAlongWithStage();

        for (int i = enemySpawner.spawnedEnemies.Count - 1; i >= 0; i--)
            enemySpawner.spawnedEnemies[i].MoveAlongWithStage();
    }

    void ChangeGameState(GameState gameState)
    {
        gameStateCur = gameState;
    }

    void MakeSchedule()
    {
        schedule.onWinTotally = () => { Invoke("SetWinBGM", WAITING_TIME_TO_PLAY_RESULT_SOUND); };
        schedule.onWinTotally += () => { Invoke("ReturnAll", WAITING_TIME_TO_PLAY_RESULT_UI); };
        schedule.onWinTotally += () => { Invoke("SetWinUI", WAITING_TIME_TO_PLAY_RESULT_UI); };
        schedule.onWinTotally += () => { Invoke("BackToStageSelectionScene", REMAINING_TIME_TO_SHOW_RESULT_UI); };

        schedule.onWin = () => { Invoke("SetWinBGM", WAITING_TIME_TO_PLAY_RESULT_SOUND); };
        schedule.onWin += () => { Invoke("ReturnAll", WAITING_TIME_TO_PLAY_RESULT_UI); };
        schedule.onWin += () => { Invoke("SetWinUI", WAITING_TIME_TO_PLAY_RESULT_UI); };
        schedule.onWin += () => { Invoke("PrepareGame", REMAINING_TIME_TO_SHOW_RESULT_UI); };
        schedule.onWin += () => { Invoke("TurnResultUIOff", REMAINING_TIME_TO_SHOW_RESULT_UI); };
        schedule.onWin += () => { Invoke("StartGame", WAITING_TIME_TO_START_GAME); };

        schedule.onFail = () => { Invoke("SetFailBGM", WAITING_TIME_TO_PLAY_RESULT_SOUND); };
        schedule.onFail += () => { Invoke("ReturnAll", WAITING_TIME_TO_PLAY_RESULT_UI); };
        schedule.onFail += () => { Invoke("SetFailUI", WAITING_TIME_TO_PLAY_RESULT_UI); };
        schedule.onFail += () => { Invoke("BackToStageSelectionScene", REMAINING_TIME_TO_SHOW_RESULT_UI); };
    }

    void ReturnAll()
    {
        enemySpawner.ReturnAll();
        itemSpawner.ReturnAll();
    }

    void BackToStageSelectionScene()
    {
        sceneController.ChangeScene(SceneState.StageSelection);
    }

    void SetWinBGM()
    {
        uiSoundPlayer.PlayBGM(BgmSoundType.Win, false);
    }

    void SetFailBGM()
    {
        uiSoundPlayer.PlayBGM(BgmSoundType.GameOver, false);
    }

    void SetWinUI()
    {
        inGameUI.SetWinUI(starCollector.GetStarCur);
    }

    void SetFailUI()
    {
        inGameUI.SetLoseUI();
    }

    void TurnResultUIOff()
    {
        inGameUI.SetInGameUI();
    }
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

public class Schedule
{
    public Action onWin;
    public Action onWinTotally;
    public Action onFail;
}
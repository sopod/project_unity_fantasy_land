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


// 인게임씬에서 게임의 흐름을 관리하는 GameCenter 클래스입니다. 
// 오브젝트들 사이를 중재하며, 게임 상태에 따라 각종 작업을 수행합니다. 


public class GameCenter : MonoBehaviour
{
    [Header("------- Obejcts")]
    [SerializeField] Player player;
    [SerializeField] Machine machine;
    [SerializeField] GameObject stageOfMachine;

    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] ItemSpawner itemSpawner;
    [SerializeField] ProjectileSpawner projectileSpawner;

    [SerializeField] InGameUIDisplay inGameUI;
    UISoundPlayer uiSoundPlayer { get => UISoundPlayer.Instance; }
    SceneController sceneController { get => SceneController.Instance; }

    [Header("------- layers")]
    [SerializeField] Layers layers;

    StageChanger stageChanger;
    StarCollector starCollector = new StarCollector();
    StageMovementValue stageVal = new StageMovementValue();
    StopWatch gameTimer = new StopWatch();
    Schedule schedule = new Schedule();

    const int limitSecondsPerStage = 180;

    const float cinemachineWaitingTime = 8.0f;




    const float resultSoundWaitingTime = 0.3f;
    const float resultUIWaitingTime = 0.4f;
    const float resultUIRemainingTime = 5.0f;
    const float gameStartWaitingTime = 5.5f;
    const float enemyStartWaitingTime = 1.0f;

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
        player.InitPlayer(stageOfMachine, stageVal, stageChanger, layers, projectileSpawner);

        machine.OnMachineMoved = MoveCreaturesAlongMachine;
        player.OnDead = OnFail;
        player.OnItemGot = (time) => { OnItemGot(time); };

        MakeSchedule();

        PrepareGame();

        MakeObjectsStopMoving();
        machine.StartMoving();

        Invoke("StartGame", cinemachineWaitingTime);
    }

    void MakeSchedule()
    {
        schedule.onWinTotally += () => { Invoke("SetWinBGM", resultSoundWaitingTime); };
        schedule.onWinTotally += () => { Invoke("SetWinUI", resultUIWaitingTime); };
        schedule.onWinTotally += () => { Invoke("BackToStageSelectionScene", resultUIRemainingTime); };

        schedule.onWinTotally += () => { Invoke("SetWinBGM", resultSoundWaitingTime); };
        schedule.onWinTotally += () => { Invoke("SetWinUI", resultUIWaitingTime); };
        schedule.onWinTotally += () => { Invoke("PrepareGame", resultUIRemainingTime); };
        schedule.onWinTotally += () => { Invoke("TurnResultUIOff", resultUIRemainingTime); };
        schedule.onWinTotally += () => { Invoke("StartGame", gameStartWaitingTime); };

        schedule.onFail += () => { Invoke("SetFailBGM", resultSoundWaitingTime); };
        schedule.onFail += () => { Invoke("SetFailUI", resultUIWaitingTime); };
        schedule.onFail += () => { Invoke("BackToStageSelectionScene", resultUIRemainingTime); };
    }
    
    void PrepareGame()
    {
        machine.ResetMachine();
        player.ResetValues();
        
        SpawnEnemies();
        SpawnItems();

        MakeObjectPaused();

        uiSoundPlayer.PlayBGM(SceneState.InGame);
    }

    void StartGame()
    {
        ChangeGameState(GameState.Playing);
        
        monsterMaxCur = stageChanger.GetMonsterMaxForCurrentStage();
        inGameUI.SetGameStartUI(limitSecondsPerStage, monsterMaxCur, stageChanger.StageCur);
        remainingMonstersCur = monsterMaxCur;

        gameTimer.StartTimer(limitSecondsPerStage);

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
            e.InitEnemy(stageOfMachine, stageVal, stageChanger, layers, projectileSpawner, player.transform);
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

        enemySpawner.ReturnAll();
        itemSpawner.ReturnAll();

        bool isWin = starCollector.SetStar(sceneController.loaderStarData, stageChanger.ModeCur, stageChanger.StageCur, remainingMonstersCur);

        if (isWin) OnWin();
        else OnFail();
    }

    public void OnWin()
    {
        MakeObjectPaused();
        ChangeGameState(GameState.ShowingResult);

        uiSoundPlayer.StopPlayingBGM();



        //Invoke("SetWinBGM", resultSoundWaitingTime);
        //Invoke("SetWinUI", resultUIWaitingTime);

        if (!stageChanger.UpgradeStage())
        {
            schedule.onWinTotally?.Invoke();
            //Invoke("BackToStageSelectionScene", resultUIRemainingTime);
            return;
        }

        schedule.onWin?.Invoke();

        //Invoke("PrepareGame", resultUIRemainingTime);
        //Invoke("TurnResultUIOff", resultUIRemainingTime);
        //Invoke("StartGame", gameStartWaitingTime);
    }

    public void OnFail()
    {
        MakeObjectPaused();
        ChangeGameState(GameState.ShowingResult);
        
        uiSoundPlayer.StopPlayingBGM();


        schedule.onFail?.Invoke();
        //Invoke("SetFailBGM", resultSoundWaitingTime);
        //Invoke("SetFailUI", resultUIWaitingTime);

        //Invoke("BackToStageSelectionScene", resultUIRemainingTime);
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

            Invoke("MakeEnemyStartMoving", enemyStartWaitingTime);
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
            EndCurrentStage();
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

    void BackToStageSelectionScene()
    {
        sceneController.ChangeScene(SceneState.StageSelection);
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

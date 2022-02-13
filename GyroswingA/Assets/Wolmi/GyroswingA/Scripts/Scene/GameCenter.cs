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
    [SerializeField] Machine machine;
    [SerializeField] Player player;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] ItemSpawner itemSpawner;
    [SerializeField] ProjectileSpawner projectileSpawner;
    [SerializeField] GameObject stageOfMachine;
    [SerializeField] InGameUIDisplay inGameUI;
    UISoundPlayer uiSoundPlayer;
    
    [Header("---- layers")]
    [SerializeField] Layers layers;

    StageChanger stageChanger;
    StarCollector starCollector = new StarCollector();
    StageMovementValue stageVal = new StageMovementValue();
    StopWatch gameTimer = new StopWatch();

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
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateInGame();
    }

    // 처음 인게임 씬으로 전환 되면 초기화하고 게임을 준비합니다. 
    void Init()
    {
        uiSoundPlayer = UISoundPlayer.Instance;

        inGameUI.gameObject.SetActive(false);

        stageChanger = new StageChanger(SceneController.Instance.loaderGoogleSheet, SceneController.Instance.loaderStarData);

        machine.Init(stageChanger, stageVal);
        player.InitPlayer(stageOfMachine, stageVal, stageChanger, layers, projectileSpawner);

        PrepareGame();

        MakeObjectsStopMoving();
        machine.StartMoving();

        Invoke("StartGame", cinemachineWaitingTime);
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

    // 게임을 시작합니다. GameState, UI, Timer를 설정하고 오브젝트들을 움직이도록합니다.  
    void StartGame()
    {
        ChangeGameState(GameState.Playing);
        
        inGameUI.gameObject.SetActive(true);

        monsterMaxCur = stageChanger.GetMonsterMaxForCurrentStage();
        inGameUI.SetGameStartUI(limitSecondsPerStage, monsterMaxCur, stageChanger.StageCur);
        remainingMonstersCur = monsterMaxCur;

        gameTimer.StartTimer(limitSecondsPerStage);

        MakeObjectsStartMoving(true);
    }

    // 게임을 업데이트합니다. 
    void UpdateInGame()
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
            GameObject e = enemySpawner.SpawnEnemyObject(typesToGen, amount);
            e.GetComponent<Enemy>().InitEnemy(stageOfMachine, stageVal, stageChanger, layers, projectileSpawner);
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
    
    // 현재 스테이지를 종료합니다. 
    void EndCurrentStage()
    {
        ChangeGameState(GameState.ShowingResult);

        enemySpawner.ReturnAllObjects();
        itemSpawner.ReturnAllObjects();

        bool isWin = starCollector.SetStar(SceneController.Instance.loaderStarData, stageChanger.ModeCur, stageChanger.StageCur, remainingMonstersCur);

        if (isWin) OnWin();
        else OnFail();
    }

    public void OnWin()
    {
        MakeObjectPaused();
        ChangeGameState(GameState.ShowingResult);

        uiSoundPlayer.StopPlayingBGM();
        Invoke("SetWinBGM", resultSoundWaitingTime);
        Invoke("SetWinUI", resultUIWaitingTime);

        if (!stageChanger.UpgradeStage())
        {
            Invoke("BackToStageSelectionScene", resultUIRemainingTime);
            return;
        }

        Invoke("PrepareGame", resultUIRemainingTime);
        Invoke("TurnResultUIOff", resultUIRemainingTime);
        Invoke("StartGame", gameStartWaitingTime);
    }

    public void OnFail()
    {
        MakeObjectPaused();
        ChangeGameState(GameState.ShowingResult);
        
        uiSoundPlayer.StopPlayingBGM();
        Invoke("SetFailBGM", resultSoundWaitingTime);
        Invoke("SetFailUI", resultUIWaitingTime);

        Invoke("BackToStageSelectionScene", resultUIRemainingTime);
    }

    // 모든 오브젝트들을 움직이게 합니다. makeEnemyWait가 true라면 잠시 몬스터의 움직임을 멈춘 후 움직이게 합니다. 
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

            Invoke("MakeEnemyStarMoving", enemyStartWaitingTime);
            return;
        }

        MakeEnemyStarMoving();
    }

    void MakeEnemyStarMoving()
    {
        for (int i = enemySpawner.spawnedEnemies.Count - 1; i >= 0; i--)
            enemySpawner.spawnedEnemies[i].StartMoving();
    }

    // 모든 오브젝트들을 Stop 시킵니다. 
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

    // 모든 오브젝트들을 Pause 시킵니다. 
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

    // 아이템을 먹으면 시간을 늘리고, UI에 표시합니다. 
    public void OnItemGot(float plusTime)
    {
        gameTimer.ExtendTimer(plusTime);
        inGameUI.NotifyItemText((plusTime > 0));
    }

    // 몬스터를 죽이면 UI에 표시하고 다 죽였다면 스테이지를 종료합니다. 
    public void OnMonsterKilled()
    {
        remainingMonstersCur--;
        inGameUI.UpdateMonsterCount(remainingMonstersCur);
        
        if (remainingMonstersCur == 0)
            EndCurrentStage();
    }

    // 머신 위의 몬스터와 플레이어가 머신과 함께 움직일 수 있도록 합니다. 
    public void MoveCreaturesAlongMachine(bool isMachineSwinging, bool isMachineSpining, bool isSpiningCW)
    {
        player.MoveAlongWithStage(isMachineSwinging, isMachineSpining, isSpiningCW);

        for (int i = enemySpawner.spawnedEnemies.Count - 1; i >= 0; i--)
            enemySpawner.spawnedEnemies[i].MoveAlongWithStage(isMachineSwinging, isMachineSpining, isSpiningCW);
    }
    
    void ChangeGameState(GameState gameState)
    {
        gameStateCur = gameState;
    }

    void BackToStageSelectionScene()
    {
        SceneController.Instance.ChangeScene(SceneState.StageSelection);
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
        inGameUI.SetGameUI();
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

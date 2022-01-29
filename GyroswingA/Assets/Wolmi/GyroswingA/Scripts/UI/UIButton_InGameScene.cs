using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton_InGameScene : UIButton
{
    [SerializeField] StarDataPerLevel _dataPerLevel;

    GameMode modeForThisButton;
    int stageNumber;
    
    void Start()
    {
        UIEventMaker.MakeButtonEvent(this);
    }

    public void SetInGameButton(GameMode mode, int stageNumber)
    {
        modeForThisButton = mode;
        this.stageNumber = stageNumber;
    }

    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);

        _dataPerLevel.levelNumberCur = stageNumber;
        _dataPerLevel.stageModeCur = modeForThisButton;

        Invoke("StartInGame", 1.0f);
    }

    void StartInGame()
    {
        SceneController.Instance.ChangeScene(SceneState.InGame);
    }
}
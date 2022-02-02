using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton_InGameScene : UIButton
{
    StarDataPerLevel starData;

    GameMode modeForThisButton;
    int stageNumber;

    void Awake()
    {
        starData = SceneController.Instance.loaderStarData.data;
    }

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

        starData.levelNumberCur = stageNumber;
        starData.stageModeCur = modeForThisButton;

        Invoke("StartInGame", 1.0f);
    }

    void StartInGame()
    {
        SceneController.Instance.ChangeScene(SceneState.InGame);
    }
}
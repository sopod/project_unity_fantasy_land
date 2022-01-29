using UnityEngine.SceneManagement;

public class UIButton_StageSelectionScene : UIButton
{
    void Start()
    {
        UIEventMaker.MakeButtonEvent(this);
    }

    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);
        SceneController.Instance.ChangeScene(SceneState.StageSelection);
    }
}

public class UIButton_StageSelectionScene : UIButton
{
    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);
        SceneLoader.LoadScene("StageSelection");
    }
}

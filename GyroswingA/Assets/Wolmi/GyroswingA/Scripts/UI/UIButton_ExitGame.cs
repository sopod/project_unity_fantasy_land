using UnityEngine;

public class UIButton_ExitGame : UIButton
{
    [SerializeField] private StarDataPerLevel starData;
    void Start()
    {
        UIEventMaker.MakeButtonEvent(this);
    }

    public override void OnClicked()
    {
        UISoundPlayer.Instance.PlayUISound(UIEffectSoundType.BtnClick);

        GameDataLoader.SaveStarDataFile(starData);

        Invoke("QuitGame", 2.0f);
    }
    
    void QuitGame()
    {
        Application.Quit();
    }
}

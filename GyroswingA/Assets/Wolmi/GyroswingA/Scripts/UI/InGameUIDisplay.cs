using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI enemyCountText;

    int monsterMax;

    [SerializeField] GameObject resultScreen;
    [SerializeField] Image resultImage;
    [SerializeField] Sprite[] resultSprite;
    [SerializeField] StarDisplay starDisplay;

    [SerializeField] GameObject optionScreen;
    [SerializeField] GameObject moblieController;

    [SerializeField] VolumeSlider BgmSlider;
    [SerializeField] VolumeSlider effectSoundSlider;



    public void SetUI(int limitTime, int enemyMax)
    {
        resultScreen.SetActive(false);
        optionScreen.SetActive(false);
        moblieController.SetActive(true);
        
        this.monsterMax = enemyMax;

        timeText.text = GetRemainingTimeString(limitTime);
        enemyCountText.text = new StringBuilder("남은 적 " + monsterMax + " / " + monsterMax).ToString();


        BgmSlider.InitSlider();
        effectSoundSlider.InitSlider();
    }

    public void UpdateTime(float remainingTime)
    {
        timeText.text = GetRemainingTimeString(remainingTime);
    }

    public void UpdateMonsterCount(int monsterCur)
    {
        enemyCountText.text = new StringBuilder("남은 적 " + monsterCur + " / " + monsterMax).ToString();
    }

    string GetRemainingTimeString(float seconds)
    {
        int sec = Mathf.FloorToInt(seconds);
        int min = 0;

        if (sec >= 60)
        {
            min = sec / 60;
            sec = sec % 60;
        }

        string res = min.ToString("00");
        res += " : ";
        res += sec.ToString("00");

        return res;
    }

    public void SetWinUI(int starsGot)
    {
        moblieController.SetActive(false);
        resultScreen.SetActive(true);
        resultImage.sprite = resultSprite[0];
        starDisplay.TurnOn(starsGot);
    }

    public void SetLoseUI()
    {
        moblieController.SetActive(false);
        resultScreen.SetActive(true);
        resultImage.sprite = resultSprite[1];
        starDisplay.TurnOff();
    }

    public void SetGameUI()
    {
        resultScreen.SetActive(false);
        optionScreen.SetActive(false);
        moblieController.SetActive(true);
    }

    public void SetOptionUI()
    {
        optionScreen.SetActive(true);
        moblieController.SetActive(false);
    }
    

}

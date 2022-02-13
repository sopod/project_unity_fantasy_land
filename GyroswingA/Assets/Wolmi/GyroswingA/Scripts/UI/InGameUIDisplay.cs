using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InGameUIDisplay : MonoBehaviour
{
    readonly string[] monsterKilledText = { "���͸� ���������� �о�¾��!", "���ϼ̾��!", "���� �ְ���!", "�̴�θ� ���� �����̿���!", "����� ���� �Ƿ���!"};
    const string speedItemText = "���ǵ� ��!";
    const string timeItemText = "���� �ð��� �þ���!";

    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI enemyCountText;
    [SerializeField] TextMeshProUGUI stageNotify;
    [SerializeField] Notify[] notifies;

    const float stageNotifyRemaingTime = 2.0f;
    const float notifyRemaingTime = 1.5f;

    int monsterMax;

    [SerializeField] GameObject resultScreen;
    [SerializeField] Image resultImage;
    [SerializeField] Sprite[] resultSprite;
    [SerializeField] StarDisplay starDisplay;

    [SerializeField] GameObject optionScreen;
    [SerializeField] GameObject moblieController;

    [SerializeField] VolumeSlider BgmSlider;
    [SerializeField] VolumeSlider effectSoundSlider;


    public void SetGameStartUI(int limitTime, int enemyMax, int level)
    {
        resultScreen.SetActive(false);
        optionScreen.SetActive(false);
        moblieController.SetActive(true);
        
        this.monsterMax = enemyMax;

        timeText.text = GetRemainingTimeString(limitTime);
        enemyCountText.text = new StringBuilder("���� �� " + monsterMax + " / " + monsterMax).ToString();


        BgmSlider.InitSlider();
        effectSoundSlider.InitSlider();

        stageNotify.gameObject.SetActive(true);
        stageNotify.text = new StringBuilder("Stage " + level).ToString();
        Invoke("OffStageNotify", stageNotifyRemaingTime);
    }

    public void UpdateTime(float remainingTime)
    {
        timeText.text = GetRemainingTimeString(remainingTime);
    }

    public void UpdateMonsterCount(int monsterCur)
    {
        enemyCountText.text = new StringBuilder("���� �� " + monsterCur + " / " + monsterMax).ToString();

        if (monsterCur != 0)
        {
            int idx = Random.Range(0, monsterKilledText.Length);
            NotifyText(monsterKilledText[idx]);
        }
    }

    public void NotifyItemText(bool isTime)
    {
        NotifyText((isTime) ? timeItemText: speedItemText);
    }

    void NotifyText(string content)
    {
        for (int i = 0; i < notifies.Length; i++)
        {
            if (!notifies[i].IsNotifying)
            {
                notifies[i].gameObject.SetActive(true);
                notifies[i].On(content, notifyRemaingTime);
                return;
            }
        }
    }

    void OffStageNotify()
    {
        stageNotify.gameObject.SetActive(false);
        stageNotify.text = "";
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

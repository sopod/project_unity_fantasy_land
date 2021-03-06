using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InGameUIDisplay : MonoBehaviour
{
    readonly string[] MONSTER_KILL_TEXTS = { "몬스터를 성공적으로 처치했어요!", "잘하셨어요!", "역시 최고에요!", "이대로만 가면 성공이에요!", "당신은 숨은 실력자!", "잘하고 있어요!", "아주 훌륭해요!"};
    const string SPEED_ITEM_TEXT = "스피드 업!";
    const string TIME_TIME_TEXT = "제한 시간이 늘어났어요!";

    [Header("------- Intro")]
    [SerializeField] GameObject introScreen;

    [Header("------- InGame")]
    [SerializeField] GameObject inGameScreen;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI enemyCountText;
    [SerializeField] GameObject moblieController;
    [SerializeField] TextMeshProUGUI stageNotify;
    [SerializeField] Notify[] notifies;

    int monsterMax;
    const float stageNotifyRemaingTime = 2.0f;
    const float notifyRemaingTime = 1.5f;

    [Header("------- Option")]
    [SerializeField] GameObject optionScreen;
    [SerializeField] VolumeSlider BgmSlider;
    [SerializeField] VolumeSlider effectSoundSlider;

    [Header("------- Result")]
    [SerializeField] GameObject resultScreen;
    [SerializeField] Image resultImage;
    [SerializeField] Sprite[] resultSprite;
    [SerializeField] StarDisplay starDisplay;

    
    public void SetGameStartUI(int limitTime, int enemyMax, int level)
    {
        SetInGameUI();

        this.monsterMax = enemyMax;

        timeText.text = GetRemainingTimeString(limitTime);
        enemyCountText.text = new StringBuilder("남은 적 " + monsterMax + " / " + monsterMax).ToString();


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
        enemyCountText.text = new StringBuilder("남은 적 " + monsterCur + " / " + monsterMax).ToString();

        if (monsterCur != 0)
        {
            int idx = Random.Range(0, MONSTER_KILL_TEXTS.Length);
            NotifyText(MONSTER_KILL_TEXTS[idx]);
        }
    }

    public void NotifyItemText(bool isTime)
    {
        NotifyText((isTime) ? TIME_TIME_TEXT: SPEED_ITEM_TEXT);
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

    public void SetInGameUI()
    {
        introScreen.SetActive(false);
        resultScreen.SetActive(false);
        optionScreen.SetActive(false);
        inGameScreen.SetActive(true);
        moblieController.SetActive(true);
    }

    public void SetOptionUI()
    {
        optionScreen.SetActive(true);
        moblieController.SetActive(false);
    }
    

}

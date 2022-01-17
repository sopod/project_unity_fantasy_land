using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI enemyCountText;
    [SerializeField] UIButton pauseButton;

    int monsterMax;
    
    [SerializeField] GameObject resultScreen;
    [SerializeField] Image resultImage;
    [SerializeField] Sprite[] resultSprite;
    [SerializeField] StarDisplay starDisplay;

    [SerializeField] GameObject optionScreen;
    [SerializeField] UIButton closeOptionButton;
    [SerializeField] UIButton lobbyButton;
    [SerializeField] UIButton exitGameButton;

    public void SetUI(int limitTime, int enemyMax)
    {
        resultScreen.SetActive(false);
        optionScreen.SetActive(false);

        UIEventMaker.MakeButtonEvent(pauseButton);
        UIEventMaker.MakeButtonEvent(closeOptionButton);
        UIEventMaker.MakeButtonEvent(lobbyButton);
        UIEventMaker.MakeButtonEvent(exitGameButton);

        this.monsterMax = enemyMax;

        timeText.text = GetRemainingTimeString(limitTime);
        enemyCountText.text = new StringBuilder("���� �� " + monsterMax + " / " + monsterMax).ToString();
    }

    public void UpdateTime(float remainingTime)
    {
        timeText.text = GetRemainingTimeString(remainingTime);
    }

    public void UpdateMonsterCount(int monsterCur)
    {
        enemyCountText.text = new StringBuilder("���� �� " + monsterCur + " / " + monsterMax).ToString();
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
        resultScreen.SetActive(true);
        resultImage.sprite = resultSprite[0];
        starDisplay.TurnOn(starsGot);
    }

    public void SetLoseUI()
    {
        resultScreen.SetActive(true);
        resultImage.sprite = resultSprite[1];
        starDisplay.TurnOff();
    }

    public void TurnOffResultUI()
    {
        resultScreen.SetActive(false);
    }

}

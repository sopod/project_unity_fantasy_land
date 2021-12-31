using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI enemyCountText;
    [SerializeField] GameObject pauseButton;

    float secondsMax;
    int monsterMax;

    public void InitUI(int secondsMax, int enemyMax)
    {
        UIEventMaker maker = new UIEventMaker();
        maker.MakeUIObjectWork(pauseButton);

        this.secondsMax = secondsMax;
        this.monsterMax = enemyMax;

        timeText.text = GetLeftTimeString(0.0f);
        enemyCountText.text = new StringBuilder("남은 적 0 / " + enemyMax).ToString();
    }

    public void UpdateTime(float curTime)
    {
        timeText.text = GetLeftTimeString(curTime);
    }

    public void UpdateMonsterCount(int monsterCur)
    {
        enemyCountText.text = new StringBuilder("남은 적 " + monsterCur + " / " + monsterMax).ToString();
    }

    string GetLeftTimeString(float seconds)
    {
        int sec = Mathf.FloorToInt(secondsMax - seconds);
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

}

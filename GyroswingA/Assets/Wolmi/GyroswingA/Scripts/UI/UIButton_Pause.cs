using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton_Pause : UIButton
{
    [SerializeField] GameManager manager;

    bool isPaused = false;

    public override void OnClicked()
    {
        isPaused = !isPaused;

        if (isPaused)
            manager.SetPauseMoving();
        else
            manager.SetStartMoving();
    }
}

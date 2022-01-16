using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton_Pause : UIButton
{
    [SerializeField] GameManager manager;
    [SerializeField] GameObject optionScreen;
    
    public override void OnClicked()
    {
        manager.SetPauseMoving();
        optionScreen.SetActive(true);
    }
}

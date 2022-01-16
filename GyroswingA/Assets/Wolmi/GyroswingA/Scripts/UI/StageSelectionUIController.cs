using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectionUIController : MonoBehaviour
{
    [SerializeField] Image modeIndicator;
    [SerializeField] Sprite[] modeImages;

    // easy mode screen
    [SerializeField] StageButtonController easyModeButtonController;
    [SerializeField] UIButton leftButtonToLobby;
    [SerializeField] UIButton rightButtonToHardMode;

    // hard mode screen
    [SerializeField] StageButtonController hardModeButtonController;
    [SerializeField] UIButton leftButtonToEasyMode;

    
    void Start()
    {
        UIEventMaker.MakeButtonEvent(leftButtonToLobby);
        UIEventMaker.MakeButtonEvent(rightButtonToHardMode);
        UIEventMaker.MakeButtonEvent(leftButtonToEasyMode);

        // set
        SetStageButtons();
        SetEasyModeUI();
    }

    void SetStageButtons()
    {
        easyModeButtonController.SetButtons();
        hardModeButtonController.SetButtons();
    }

    public void SetEasyModeUI()
    {
        modeIndicator.sprite = modeImages[0];

        leftButtonToLobby.gameObject.SetActive(true);
        rightButtonToHardMode.gameObject.SetActive(true);
        leftButtonToEasyMode.gameObject.SetActive(false);

        easyModeButtonController.gameObject.SetActive(true);
        hardModeButtonController.gameObject.SetActive(false);
    }

    public void SetHardModeUI()
    {
        modeIndicator.sprite = modeImages[1];

        leftButtonToLobby.gameObject.SetActive(false);
        rightButtonToHardMode.gameObject.SetActive(false);
        leftButtonToEasyMode.gameObject.SetActive(true);

        easyModeButtonController.gameObject.SetActive(false);
        hardModeButtonController.gameObject.SetActive(true);
    }

}

using UnityEngine;
using UnityEngine.UI;

public class StageSelectionUIDisplay : MonoBehaviour
{
    [SerializeField] Image modeIndicator;
    [SerializeField] Sprite[] modeImages;

    // easy mode screen
    [SerializeField] StageButtonDisplay _easyModeButtonDisplay;
    [SerializeField] UIButton leftButtonToLobby;
    [SerializeField] UIButton rightButtonToHardMode;

    // hard mode screen
    [SerializeField] StageButtonDisplay _hardModeButtonDisplay;
    [SerializeField] UIButton leftButtonToEasyMode;

    
    void Start()
    {
        SetStageButtons();
        SetEasyModeUI();
    }

    void SetStageButtons()
    {
        _easyModeButtonDisplay.SetButtons();
        _hardModeButtonDisplay.SetButtons();
    }

    public void SetEasyModeUI()
    {
        modeIndicator.sprite = modeImages[0];

        leftButtonToLobby.gameObject.SetActive(true);
        rightButtonToHardMode.gameObject.SetActive(true);
        leftButtonToEasyMode.gameObject.SetActive(false);

        _easyModeButtonDisplay.gameObject.SetActive(true);
        _hardModeButtonDisplay.gameObject.SetActive(false);
    }

    public void SetHardModeUI()
    {
        modeIndicator.sprite = modeImages[1];

        leftButtonToLobby.gameObject.SetActive(false);
        rightButtonToHardMode.gameObject.SetActive(false);
        leftButtonToEasyMode.gameObject.SetActive(true);

        _easyModeButtonDisplay.gameObject.SetActive(false);
        _hardModeButtonDisplay.gameObject.SetActive(true);
    }

}

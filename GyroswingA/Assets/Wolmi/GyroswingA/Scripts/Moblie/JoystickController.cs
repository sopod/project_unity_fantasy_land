using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    const float leverRange = 120.0f;
    const float screenResolusionX = 1920f;
    const float screenResolusionY = 1080f;


    RectTransform joystick;
    [SerializeField] RectTransform lever;

    Vector2 inputDir;
    bool isInput;
    public bool IsInput { get { return isInput; } }


    void Awake()
    {
        joystick = GetComponent<RectTransform>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        SetLeverPosition(eventData);
        isInput = true; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetLeverPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetLeverPosition();
        isInput = false;
    }

    void SetLeverPosition(PointerEventData eventData)
    {
        float x = (Screen.width * joystick.sizeDelta.x / 2 + joystick.anchoredPosition.x) / screenResolusionX; // 245f
        float y = (Screen.height * joystick.sizeDelta.y / 2 + joystick.anchoredPosition.y) / screenResolusionY;
        
        var leverPos = eventData.position - new Vector2(x, y);
        var inRangePos = (leverPos.magnitude < leverRange) ? leverPos : leverPos.normalized * leverRange;
        lever.anchoredPosition = inRangePos;

        inputDir = inRangePos / leverRange;
    }

    void ResetLeverPosition()
    {
        lever.anchoredPosition = Vector2.zero;
    }

    public float GetXDir()
    {
        if (isInput)
            return inputDir.x;

        return 0.0f;
    }

    public float GetYDir()
    {
        if (isInput)
            return inputDir.y;

        return 0.0f;
    }
    

}

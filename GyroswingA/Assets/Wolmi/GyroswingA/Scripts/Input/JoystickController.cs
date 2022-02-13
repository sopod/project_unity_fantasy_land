using UnityEngine;
using UnityEngine.EventSystems;


// 조이스틱 조작을 위한 JoystickController 클래스입니다. 


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
        // 화면 크기 별로 달라지는 터치 위치를 보정하기 위한 코드입니다. 
        float x = (Screen.width * joystick.sizeDelta.x / 2 + joystick.anchoredPosition.x) / screenResolusionX;
        float y = (Screen.height * joystick.sizeDelta.y / 2 + joystick.anchoredPosition.y) / screenResolusionY;
        
        var leverPos = eventData.position - new Vector2(x, y);

        // leverRange 이상으로 조이스틱이 화면에서 넘어가지 않게 만들기 위한 코드입니다. 
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
        return (isInput) ? inputDir.x : 0.0f;
    }

    public float GetYDir()
    {
        return (isInput) ? inputDir.y : 0.0f;
    }
}
using UnityEngine;
using UnityEngine.EventSystems;



public class JoystickController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    const float LEVER_RANGE = 120.0f;
    const float SCREEN_RESOLUSION_X = 1920f;
    const float SCREEN_RESOLUSION_Y = 1080f;

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
        float x = (Screen.width * joystick.sizeDelta.x / 2 + joystick.anchoredPosition.x) / SCREEN_RESOLUSION_X;
        float y = (Screen.height * joystick.sizeDelta.y / 2 + joystick.anchoredPosition.y) / SCREEN_RESOLUSION_Y;
        
        Vector2 leverPos = eventData.position - new Vector2(x, y);

        Vector2 inRangePos = (leverPos.magnitude < LEVER_RANGE) ? leverPos : leverPos.normalized * LEVER_RANGE;
        lever.anchoredPosition = inRangePos;

        inputDir = inRangePos / LEVER_RANGE;
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
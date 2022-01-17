using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    const float leverRange = 120.0f;

    RectTransform joystick;
    [SerializeField] RectTransform lever;

    Vector2 inputDir;
    bool isInput;
    public bool IsInput { get { return isInput; } }


    void Awake()
    {
        joystick = GetComponent<RectTransform>();
    }

    //void Update()
    //{
    //    if (isInput)
    //        Debug.Log(inputDir.x + " / " + inputDir.y);
    //}

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetLeverPosition(eventData);
        isInput = true; 
    }

    public void OnDrag(PointerEventData eventData) // call when mouse is moving...
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
        var leverPos = eventData.position - joystick.anchoredPosition;
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

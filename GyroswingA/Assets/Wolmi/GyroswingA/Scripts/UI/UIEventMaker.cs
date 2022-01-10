using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIEventMaker
{
    UIButton uiBtn;
    Button btn;
    EventTrigger trigger;

    public bool MakeUIObjectWork(UIButton obj)
    {
        uiBtn = obj;
        btn = obj.GetComponent<Button>();
        trigger = obj.GetComponent<EventTrigger>();

        if (trigger == null || btn == null) return false;

        SetEvents();

        return true;
    }

    void SetEvents()
    {
        trigger.triggers.Clear();
        
        AddEvent(EventTriggerType.PointerClick, (data) => { OnLeftClick((PointerEventData)data); });
    }

    void AddEvent(EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = type;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }   

    void ClearEvent()
    {
        trigger.triggers.Clear();
    }    

    void OnLeftClick(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            uiBtn.OnClicked();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIEventMaker
{
    public bool MakeUIObjectWork(UIButton obj)
    {
        Button btn = obj.GetComponent<Button>();
        EventTrigger trigger = obj.GetComponent<EventTrigger>();

        if (trigger == null || btn == null) return false;

        SetEvents(trigger, obj);

        return true;
    }

    void SetEvents(EventTrigger trigger, UIButton obj)
    {
        ClearEvent(trigger);
        
        AddEvent(trigger, EventTriggerType.PointerClick, (data) => { OnLeftClick((PointerEventData)data, obj); });
    }

    void AddEvent(EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = type;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }   

    void ClearEvent(EventTrigger trigger)
    {
        trigger.triggers.Clear();
    }    

    void OnLeftClick(PointerEventData data, UIButton obj)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            obj.OnClicked();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIEventMaker
{
    Button btn;
    EventTrigger trigger;

    public bool MakeUIObjectWork(GameObject obj)
    {
        btn = obj.GetComponent<Button>();
        trigger = obj.GetComponent<EventTrigger>();

        if (trigger == null || btn == null) return false;

        SetEvents();

        return true;
    }

    void SetEvents()
    {
        trigger.triggers.Clear();
        
        AddEvent(EventTriggerType.PointerClick, (data) => { OnRightClick((PointerEventData)data); });
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

    void OnRightClick(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            //Debug.Log("clicked");
            
        }
    }
}

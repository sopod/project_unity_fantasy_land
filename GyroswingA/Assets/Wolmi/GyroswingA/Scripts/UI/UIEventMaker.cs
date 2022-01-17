using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UIEventMaker
{
    public static bool MakeButtonEvent(UIButton obj)
    {
        Button btn = obj.GetComponent<Button>();
        EventTrigger trigger = obj.GetComponent<EventTrigger>();

        if (trigger == null || btn == null) return false;

        SetClickEvent(trigger, obj);

        return true;
    }
    
    static void SetClickEvent(EventTrigger trigger, UIButton obj)
    {
        ClearEvent(trigger);
        
        AddEvent(trigger, EventTriggerType.PointerClick, (data) => { OnLeftClick((PointerEventData)data, obj); });
    }

    static void AddEvent(EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = type;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    static void ClearEvent(EventTrigger trigger)
    {
        trigger.triggers.Clear();
    }

    static void OnLeftClick(PointerEventData data, UIButton obj)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            obj.OnClicked();
        }
    }
}

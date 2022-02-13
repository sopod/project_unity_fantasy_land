using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// Button의 UI 이벤트를 코드로 작성하여 연결하였습니다.
// 유니티 에디터 상에서 연결하지 않은 이유는, 코드로 작성함으로써 반복적인 연결 작업을 간소화할 수 있기 때문입니다. 


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

    static void ClearEvent(EventTrigger trigger)
    {
        trigger.triggers.Clear();
    }

    static void AddEvent(EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = type;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    static void OnLeftClick(PointerEventData data, UIButton obj)
    {
        if (data.button == PointerEventData.InputButton.Left)
            obj.OnClicked();
    }
}

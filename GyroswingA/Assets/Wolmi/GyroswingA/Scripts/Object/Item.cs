using UnityEngine;


public class Item : MovingThing, ISpawnableObject
{
    public event BackToPoolDelegate BackToPool;
    public void InvokeBackToPool() { BackToPool?.Invoke(); }

    [SerializeField] ItemType itemType = ItemType.Max;
    public int Type
    {
        get => (int)itemType;
        set
        {
            if (value >= (int)ItemType.Max) return;
            itemType = (ItemType)value;
        }
    }

}

using UnityEngine;

public class Item : MovingThing, ISpawnableObject
{
    ItemType itemType;
    public int Type
    {
        get => (int)itemType;
        set { itemType = (ItemType)value; }
    }
}

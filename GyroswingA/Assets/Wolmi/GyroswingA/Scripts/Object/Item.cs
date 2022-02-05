using UnityEngine;

public class Item : MovingThing, ISpawnableObject
{
    ItemType itemType;
    public int Type
    {
        get => (int)itemType;
        set { itemType = (ItemType)value; }
    }

    Options options;

    public void SetItem(Options options)
    {
        this.options = options;
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPaused) return;

        int layer = (1 << other.gameObject.layer);
        if (layer != options.PlayerLayer.value) return;

        options.OnPlayerSpeedItemUsed(itemType);

        float plusTime = options.GetItemSecondsToAdd(itemType);
        GameCenter.Instance.OnTimeItemUsed(plusTime);

        gameObject.SetActive(false);
    }
}

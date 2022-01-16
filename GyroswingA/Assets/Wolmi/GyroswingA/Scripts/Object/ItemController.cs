using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MovingThing, ISpawnableObject
{
    ItemType itemType;
    public int Type
    {
        get { return (int)itemType; }
        set { itemType = (ItemType)value; }
    }

    Options options;
    StageMovementValue stageVal;
    protected GameObject stage;

    public void SetItem(GameObject stage, StageMovementValue stageVal, Options options)
    {
        this.stage = stage;
        this.stageVal = stageVal;
        this.options = options;
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPaused()) return;

        int layer = (1 << other.gameObject.layer);

        if (layer == options.PlayerLayer.value)
        {
            // effect




            this.gameObject.SetActive(false);
        }
    }
}

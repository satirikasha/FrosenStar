using UnityEngine;
using System.Collections;
using System;

public abstract class SlotItemTemplate<T> : InventoryItemTemplate<T> where T : SlotItem {
    [Header("Slot")]
    public ShipPart ItemPrefab;

    protected override void SetItemValues(ref T item) {
        base.SetItemValues(ref item);
        item.ItemPrefab = ItemPrefab;
    }
}

[Serializable]
public abstract class SlotItem : InventoryItem {
    public ShipPart ItemPrefab;

    public abstract bool CheckCompatability(ItemSlot.SlotType type);

    public ShipPart Instantiate() {
        var item = GameObject.Instantiate<ShipPart>(ItemPrefab);
        item.Item = this;
        return item;
    }
}


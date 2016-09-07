using UnityEngine;
using System.Collections;
using System;

public abstract class SlotItemTemplate<T> : InventoryItemTemplate<T> where T : SlotItem {
    [Header("Slot")]
    public ShipItem ItemPrefab;

    protected override void SetItemValues(ref T item) {
        base.SetItemValues(ref item);
        item.ItemPrefab = ItemPrefab;
    }
}

[Serializable]
public abstract class SlotItem : InventoryItem {
    public ShipItem ItemPrefab;

    public abstract bool CheckCompatability(ItemSlot.SlotType type);

    public ShipItem Instantiate() {
        var item = GameObject.Instantiate<ShipItem>(ItemPrefab);
        item.Item = this;
        return item;
    }
}


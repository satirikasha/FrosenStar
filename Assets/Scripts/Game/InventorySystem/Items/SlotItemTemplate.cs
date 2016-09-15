using UnityEngine;
using System.Collections;
using System;

public abstract class SlotItemTemplate<T> : InventoryItemTemplate<T> where T : SlotItem {
    [Header("Slot")]
    public GameObject ItemPrefab;

    protected override void SetItemValues(ref T item) {
        base.SetItemValues(ref item);
        item.ItemPrefab = ItemPrefab;
    }
}

[Serializable]
public abstract class SlotItem : InventoryItem {
    public GameObject ItemPrefab;

    public abstract bool CheckCompatability(ItemSlot.SlotType type);

    public ShipPart Instantiate() {
        var item = GameObject.Instantiate(ItemPrefab).GetComponent<ShipPart>();
        item.Item = this;
        return item;
    }
}


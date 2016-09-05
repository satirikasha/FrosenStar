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

public abstract class SlotItem : InventoryItem {
    public GameObject ItemPrefab;
}


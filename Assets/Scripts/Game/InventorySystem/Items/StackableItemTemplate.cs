using UnityEngine;
using System.Collections;
using System;

public abstract class StackableItemTemplate<T> : InventoryItemTemplate<T> where T : StackableItem {
    [Header("Stack")]
    public int Quantity;

    protected override void SetItemValues(ref T item) {
        base.SetItemValues(ref item);
        item.Quantity = Quantity;
    }
}

[Serializable]
public class StackableItem : InventoryItem {
    public int Quantity;
}


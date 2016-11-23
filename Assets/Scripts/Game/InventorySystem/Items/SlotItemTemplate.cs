using UnityEngine;
using System.Collections;
using System;
using System.Linq;

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
    public int EquipedSlotID = -1;

    public abstract bool CheckCompatability(SlotType type);

    public ItemSlot GetSlot() {
        return PlayerController.LocalPlayer.Ship.ItemSlots.FirstOrDefault(s => s.ID == EquipedSlotID);
    }

    public ShipPart Instantiate() {
        var item = GameObject.Instantiate(ItemPrefab).GetComponent<ShipPart>();
        item.Item = this;
        return item;
    }
}


using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Cargo.asset", menuName = "Inventory System/Cargo", order = 0)]
public class CargoItemTemplate : SlotItemTemplate<CargoItem> {
    public float Capacity = 100f;

    protected override void SetItemValues(ref CargoItem item) {
        base.SetItemValues(ref item);
        item.Capacity = Capacity;
    }
}

[Serializable]
public class CargoItem : SlotItem {
    public float Capacity;

    public override bool CheckCompatability(SlotType type) {
        return type == SlotType.Cargo;
    }
}


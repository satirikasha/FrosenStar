using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Container.asset", menuName = "Inventory System/Container", order = 0)]
public class ContainerItemTemplate : SlotItemTemplate<ContainerItem> {
    public float Capacity = 100f;

    protected override void SetItemValues(ref ContainerItem item) {
        base.SetItemValues(ref item);
        item.Capacity = Capacity;
    }
}

[Serializable]
public class ContainerItem : SlotItem {
    public float Capacity;

    public override bool CheckCompatability(SlotType type) {
        return type == SlotType.Container;
    }
}


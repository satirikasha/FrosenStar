using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Engine.asset", menuName = "Inventory System/Engine", order = 0)]
public class EngineItemTemplate : SlotItemTemplate<EngineItem> {

    [Header("Engine")]
    public float EnergyConsumption = 0.25f;
    public float Thrust = 750f;

    protected override void SetItemValues(ref EngineItem item) {
        base.SetItemValues(ref item);
        item.EnergyConsumption = EnergyConsumption;
        item.Thrust = Thrust;
    }
}

[Serializable]
public class EngineItem : SlotItem {
    public float EnergyConsumption;
    public float Thrust;

    public override bool CheckCompatability(ItemSlot.SlotType type) {
        return type == ItemSlot.SlotType.Engine;
    }
}


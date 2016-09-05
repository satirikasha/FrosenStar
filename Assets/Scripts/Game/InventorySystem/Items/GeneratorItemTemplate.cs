using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Generator.asset", menuName = "Inventory System/Tools/Generator", order = 0)]
public class GeneratorItemTemplate : ToolItemTemplate<GeneratorItem> {
    [Header("Generator")]
    public float EnergyRegeneration = 0.75f;

    protected override void SetItemValues(ref GeneratorItem item) {
        base.SetItemValues(ref item);
        item.EnergyRegeneration = EnergyRegeneration;
    }
}

[Serializable]
public class GeneratorItem : ToolItem {
    public float EnergyRegeneration;
}


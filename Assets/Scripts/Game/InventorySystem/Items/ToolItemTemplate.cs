using UnityEngine;
using System.Collections;
using System;

public abstract class ToolItemTemplate<T> : SlotItemTemplate<T> where T : ToolItem {}

[Serializable]
public abstract class ToolItem : SlotItem {
    public override bool CheckCompatability(SlotType type) {
        return type == SlotType.Tool;
    }
}


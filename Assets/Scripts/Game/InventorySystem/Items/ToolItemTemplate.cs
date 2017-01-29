using UnityEngine;
using System.Collections;
using System;

public abstract class ToolItemTemplate<T> : SlotItemTemplate<T> where T : ToolItem {}

[Serializable]
public abstract class ToolItem : SlotItem {

    public override SlotType Type {
        get {
            return SlotType.Tool;
        }
    }
}


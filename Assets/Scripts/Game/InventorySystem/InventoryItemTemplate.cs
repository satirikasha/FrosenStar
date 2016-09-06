using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;


public abstract class InventoryItemTemplate<T> : InventoryItemTemplate where T : InventoryItem {

    public sealed override Type InventoryItemType {
        get {
            return typeof(T);
        }
    }

    protected sealed override void SetItemValues<I>(ref I item) {
        base.SetItemValues(ref item);
    }

    protected virtual void SetItemValues(ref T item) {
        base.SetItemValues(ref item);
    }
}

public class InventoryItemTemplate : ScriptableObject {

    public virtual Type InventoryItemType { get { return typeof(InventoryItem); } }

    [Header("Info")]
    public string Name;
    public Texture2D Preview;
    [Multiline]
    public string Description;

    public InventoryItem GenerateItem() {
        var item = (InventoryItem)Activator.CreateInstance(InventoryItemType);
        SetItemValues(ref item);
        return item;
    }

    protected virtual void SetItemValues<I>(ref I item) where I: InventoryItem {
        item.Name = Name;
        item.Preview = Preview;
        item.Description = Description;
    }
}

[Serializable]
public class InventoryItem {
    public string Name;
    public Texture2D Preview;
    [Multiline]
    public string Description;
}

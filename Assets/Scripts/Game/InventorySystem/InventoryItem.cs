using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

//[CreateAssetMenu(fileName = "Item.asset", menuName = "Inventory System/Items/Generic", order = 1000)]
public abstract class InventoryItem: ScriptableObject {
    public ItemSlot.SlotType SlotType;
    public string Name;
    public Texture2D Preview;
    [Multiline]
    public string Description;
}

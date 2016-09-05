using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Generic.asset", menuName = "Inventory System/Generic", order = 0)]
public class GenericItemTemplate : InventoryItemTemplate<GenericItem> { }

public class GenericItem : InventoryItem { }

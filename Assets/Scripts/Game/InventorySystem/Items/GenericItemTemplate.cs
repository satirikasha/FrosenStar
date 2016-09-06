using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Generic.asset", menuName = "Inventory System/Generic", order = 0)]
public class GenericItemTemplate : StackableItemTemplate<GenericItem> { }

public class GenericItem : StackableItem { }

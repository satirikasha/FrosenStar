using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Generic.asset", menuName = "Inventory System/Generic", order = 0)]
public class GenericItem : InventoryItem {
    public float EnergyConsumption = 0.5f;
    public float Cooldown = 0.15f;
}

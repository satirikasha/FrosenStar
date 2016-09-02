using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Cargo.asset", menuName = "Inventory System/Cargo", order = 0)]
public class CargoItem : InventoryItem {
    public float EnergyConsumption = 0.5f;
    public float Cooldown = 0.15f;
}

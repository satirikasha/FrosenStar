using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Engine.asset", menuName = "Inventory System/Engine", order = 0)]
public class EngineItem : InventoryItem {
    public float EnergyConsumption = 0.5f;
    public float Cooldown = 0.15f;
}

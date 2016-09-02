using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Weapon.asset", menuName = "Inventory System/Weapon", order = 0)]
public class WeaponItem : InventoryItem {
    public float EnergyConsumption = 0.5f;
    public float Cooldown = 0.15f;
}

using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Weapon.asset", menuName = "Inventory System/Weapon", order = 0)]
public class WeaponItemTemplate : SlotItemTemplate<WeaponItem> {

    [Header("Weapon")]
    public float EnergyConsumption = 0.5f;
    public float Cooldown = 0.15f;
    public float Damage = 0.5f;

    protected override void SetItemValues(ref WeaponItem item) {
        base.SetItemValues(ref item);
        item.EnergyConsumption = EnergyConsumption;
        item.Cooldown = Cooldown;
        item.Damage = Damage;
    }
}

[Serializable]
public class WeaponItem : SlotItem {
    public float EnergyConsumption;
    public float Cooldown;
    public float Damage;

    public override SlotType Type {
        get {
            return SlotType.Weapon;
        }
    }
}


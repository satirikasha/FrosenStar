using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Ship.asset", menuName = "Inventory System/Ship", order = 0)]
public class ShipItemTemplate : InventoryItemTemplate<ShipItem> {
    [Header("Ship")]
    public GameObject ShipPrefab;
    public float Health;
    public float Energy;
    public float Mass;
    public float Handling;

    protected override void SetItemValues(ref ShipItem item) {
        base.SetItemValues(ref item);
        item.ShipPrefab = ShipPrefab;
        item.Health = Health;
        item.Energy = Energy;
        item.Mass = Mass;
        item.Handling = Handling;
    }
}

[Serializable]
public class ShipItem : InventoryItem {
    public GameObject ShipPrefab;
    public float Health;
    public float Energy;
    public float Mass;
    public float Handling;

    public ShipController Instantiate(bool enemy) {
        var ship = GameObject.Instantiate(ShipPrefab).GetComponent<ShipController>();
        ship.Item = this;
        if (enemy) {
            ship.gameObject.AddComponent<ShipAIController>();
        }
        else {
            ship.gameObject.AddComponent<PlayerController>();
        }
        return ship;
    }
}


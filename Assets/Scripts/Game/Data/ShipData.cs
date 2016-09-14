using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ShipData : IData {

    public ShipItem ShipItem;
    public Vector3 Position;
    public Quaternion Rotation;
    public float Health;
    public float Energy;

    public void GatherData() {
        ShipItem = PlayerController.LocalPlayer.Ship.Item;
        Health = PlayerController.LocalPlayer.Ship.Health;
        Energy = PlayerController.LocalPlayer.Ship.Energy;
        Debug.Log(Position);
    }

    public void ScatterData() {
        var ship = ShipItem.Instantiate();
        ship.transform.position = Position;
        ship.transform.rotation = Rotation;
        ship.SetHealth(Health);
        ship.SetEnergy(Energy);
    }
}

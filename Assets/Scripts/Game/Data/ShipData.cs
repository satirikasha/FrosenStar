using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ShipData : IData {

    public InventoryData InventoryData = new InventoryData();

    public ShipItem ShipItem;
    public Vector3 Position;
    public Quaternion Rotation;

    public void GatherData() {
        ShipItem = PlayerController.LocalPlayer.Ship.Item;
        InventoryData.GatherData();
    }

    public void ScatterData() {
        //var ship = ShipItem.Instantiate();
        //PlayerController.LocalPlayer.Ship.transform.position = Position;
        //PlayerController.LocalPlayer.Ship.transform.rotation = Rotation;
        InventoryData.ScatterData();
    }
}

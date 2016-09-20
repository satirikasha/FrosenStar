using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ShipData : IData {

    public ShipItem ShipItem;
    public Vector3 Position;
    public Quaternion Rotation;

    public void GatherData() {
        ShipItem = PlayerController.LocalPlayer.Ship.Item;
    }

    public void ScatterData() {
    }
}

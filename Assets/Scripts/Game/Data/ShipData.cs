using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ShipData : IData {

    public float Health;
    public float Energy;

    public void RefreshData() {
        Health = PlayerController.LocalPlayer.Ship.Health;
        Energy = PlayerController.LocalPlayer.Ship.Energy;
    }
}

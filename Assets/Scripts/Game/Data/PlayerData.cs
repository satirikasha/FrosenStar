using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerData: IData {
    public ShipData ShipData = new ShipData();

    public void GatherData() {
        ShipData.GatherData();
    }

    public void ScatterData() {
        ShipData.ScatterData();
    }
}

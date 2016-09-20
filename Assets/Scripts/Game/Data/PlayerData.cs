using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerData: IData {
    public ShipData ShipData = new ShipData();
    public InventoryData InventoryData = new InventoryData();

    public void GatherData() {
        ShipData.GatherData();
        InventoryData.GatherData();
    }

    public void ScatterData() {
        ShipData.ScatterData();
        InventoryData.ScatterData();
    }
}

using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerData: IData {
    public ShipData ShipData { get; set; }

    public void RefreshData() {
        ShipData.RefreshData();
    }
}

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class InventoryData : IData {

    public List<InventoryItem> InventoryItems;

    public void GatherData() {
        InventoryItems = PlayerController.LocalPlayer.Ship.Inventory.Items;
    }

    public void ScatterData() { }
}

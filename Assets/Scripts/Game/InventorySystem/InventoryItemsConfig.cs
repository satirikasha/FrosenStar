using UnityEngine;
using System.Collections;
using Tools;
using System.Collections.Generic;
using System.Linq;

public static class InventoryItemsConfig {
    public static List<InventoryItem> Items {
        get {
            if (_Items == null)
                RefreshItems();
            return _Items;
        }
    }
    private static List<InventoryItem> _Items;

    public static void RefreshItems() {
        _Items = Resources.LoadAll<InventoryItem>("Items").ToList();
        Debug.Log("Items refreshed: " + _Items.Count);
    }

    public static List<string> GetItemNames() {
        return Items.Select(_ => _.Name).ToList();
    }
}

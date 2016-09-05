using UnityEngine;
using System.Collections;
using Tools;
using System.Collections.Generic;
using System.Linq;

public static class InventoryItemsConfig {
    public static List<InventoryItemTemplate> Items {
        get {
            if (_Items == null)
                RefreshItems();
            return _Items;
        }
    }
    private static List<InventoryItemTemplate> _Items;

    public static void RefreshItems() {
        _Items = Resources.LoadAll<InventoryItemTemplate>("Items").ToList();
    }

    public static List<string> GetItemNames() {
        return Items.Select(_ => _.Name).ToList();
    }
}

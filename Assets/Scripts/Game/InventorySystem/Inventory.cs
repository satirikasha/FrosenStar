using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public List<InventoryItem> Items {
        get {
            if (_Items == null)
                _Items = new List<InventoryItem>();
            return _Items;
        }
    }
    private List<InventoryItem> _Items;

    public static void AddItem(InventoryItem item, Inventory target) {
        target.Items.Add(item);
    }

    public static void AddItems(IEnumerable<InventoryItem> items, Inventory target) {
        target.Items.AddRange(items);
    }

    public static void TransitItem(InventoryItem item, Inventory from, Inventory to) {
        if (from.Items.Remove(item)) {
            to.Items.Add(item);
        }
    }

    public static void TransitAll(Inventory from, Inventory to) {
        to.Items.AddRange(from.Items);
        from.Items.Clear();
    }
}

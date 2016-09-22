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
    [SerializeField]
    private List<InventoryItem> _Items;

    public static void AddItem(InventoryItem item, Inventory target) {
        target.Items.Add(item);
    }

    public static void AddItems(IEnumerable<InventoryItem> items, Inventory target) {
        foreach (var item in items) {
            AddItem(item, target);
        }
    }

    public static bool RemoveItem(InventoryItem item, Inventory target) {
        return target.Items.Remove(item);
    }

    public static void TransitItem(InventoryItem item, Inventory from, Inventory to) {
        if (RemoveItem(item, from)) {
            AddItem(item, to);
        }
    }

    public static void TransitItems(Inventory from, Inventory to) {
        AddItems(from.Items, to);
        ClearItems(from);
    }

    public static void ClearItems(Inventory target) {
        target.Items.Clear();
    }
}

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
    [SerializeField] //Debug
    private List<InventoryItem> _Items;

    public static void AddItem(InventoryItem item, Inventory target) {
        if (item != null && target != null) {
            target.Items.Add(item);
        }
    }

    public static void AddItems(IEnumerable<InventoryItem> items, Inventory target) {
        if (target != null) {
            foreach (var item in items) {
                AddItem(item, target);
            }
        }
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

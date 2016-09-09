using UnityEngine;
using System.Collections;
using UI.Markers;

[RequireComponent(typeof(SlotMarkerProvider))]
public class ItemSlot : MonoBehaviour {

    public enum SlotType {
        Tool,
        Weapon,
        Engine, 
        Cargo
    }

    public SlotType Type;
    public InventoryItemTemplate DefaultItem;

    public ShipController Ship { get; private set; }
    public ShipItem Item { get; private set; }

    void Start() {
        Ship = this.transform.GetComponentInParent<ShipController>();
        // TODO: Check if slot has allready been initialized
        if (DefaultItem != null) {
            var item = DefaultItem.GenerateItem() as SlotItem;
            Inventory.AddItem(item, Ship.Inventory);
            Equip(item);
        }
    }

    public bool Equip(SlotItem item) {
        if (item != null && item.CheckCompatability(Type)) {
            var shipItem = item.Instantiate();
            shipItem.transform.SetParent(this.transform);
            shipItem.transform.localPosition = Vector3.zero;
            shipItem.transform.localRotation = Quaternion.identity;
            shipItem.transform.localScale    = Vector3.one;
            Item = shipItem;
            Item.SetSlot(this);
        }
        return false;
    }
}

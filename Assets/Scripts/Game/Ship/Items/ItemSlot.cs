using UnityEngine;
using System.Collections;
using UI.Markers;
using System.Linq;

[RequireComponent(typeof(SlotMarkerProvider))]
public class ItemSlot : MonoBehaviour {

    public int ID {
        get {
            if (_ID < 0)
                _ID = this.transform.GetSiblingIndex();
            return _ID;
        }
    }
    private int _ID = -1;

    public SlotType Type;
    public InventoryItemTemplate DefaultItem;

    public ShipController Ship {
        get {
            if(_Ship == null)
               _Ship = this.transform.GetComponentInParent<ShipController>();
            return _Ship;
        }
    }
    private ShipController _Ship;

    public ShipPart ShipPart { get; private set; }

    public void Construct() {
        if (DefaultItem != null) {
            var item = DefaultItem.GenerateItem() as SlotItem;
            Debug.Log(Ship.Inventory);
            Inventory.AddItem(item, Ship.Inventory);
            Equip(item);
        }
    }

    public bool Equip(SlotItem item) {
        if (item != null && item.CheckCompatability(Type)) {
            item.EquipedSlotID = ID;
            ShipPart = item.Instantiate();
            ShipPart.transform.SetParent(this.transform);
            ShipPart.transform.localPosition = Vector3.zero;
            ShipPart.transform.localRotation = Quaternion.identity;
            ShipPart.transform.localScale    = Vector3.one;
            ShipPart.SetSlot(this);
        }
        return false;
    }
}

public enum SlotType {
    Tool,
    Weapon,
    Engine,
    Cargo
}
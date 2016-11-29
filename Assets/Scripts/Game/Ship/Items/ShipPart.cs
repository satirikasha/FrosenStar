using UnityEngine;
using System.Collections;

public abstract class ShipPart : MonoBehaviour {

    public InventoryItem Item;

    protected ItemSlot Slot { get; private set; }

    protected ShipController Ship {
        get
        {
            return Slot.Ship;
        }
    }

    public void SetSlot(ItemSlot slot) {
        Slot = slot;
    }

}

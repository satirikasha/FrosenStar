using UnityEngine;
using System.Collections;

public abstract class ShipItem : MonoBehaviour {

    [InventoryItemID]
    public string ItemID;

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

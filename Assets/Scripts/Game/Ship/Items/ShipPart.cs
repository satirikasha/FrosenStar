using UnityEngine;
using System.Collections;

public abstract class ShipPart : MonoBehaviour {

    public SlotItem Item;

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

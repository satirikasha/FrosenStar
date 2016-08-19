using UnityEngine;
using System.Collections;

public class ItemSlot : MonoBehaviour {

    public enum SlotType {
        Tool,
        Weapon,
        Engine, 
        Cargo
    }

    public SlotType Type;
    public ShipController Ship { get; private set; }
    public ShipItem Item { get; private set; }

    void Awake() {
        Ship = this.transform.GetComponentInParent<ShipController>();
        Item = this.transform.GetChild(0).GetComponent<ShipItem>();
        Item.SetSlot(this);
    }
}

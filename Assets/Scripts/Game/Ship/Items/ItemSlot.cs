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
    public ShipItem Item { get; private set; }

    void Awake() {
        Item = this.transform.GetChild(0).GetComponent<ShipItem>();
    }
}

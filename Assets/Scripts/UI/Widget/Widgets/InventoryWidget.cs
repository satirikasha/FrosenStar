using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class InventoryWidget : MonoBehaviour {

    public bool UseSlotFilter;
    [ReadOnly("UseSlotFilter")]
    public SlotType SlotFilter;
    [ReadOnly("UseSlotFilter", false)]
    public bool CargoOnly;

    void Start() {
        Refresh();
    }

    public void Refresh() {
        if (UseSlotFilter) {
            var items = PlayerController.LocalPlayer.Inventory.Items.OfType<SlotItem>().Where(_ => _.CheckCompatability(SlotFilter)).ToList();
            items.ForEach(_ => ItemPreviewWidget.Instantiate(_, this.transform));
        }
        else {
            if (CargoOnly) {
                var items = PlayerController.LocalPlayer.Inventory.Items.OfType<GenericItem>().ToList();
                items.ForEach(_ => ItemPreviewWidget.Instantiate(_, this.transform));
            }
            else {
                var items = PlayerController.LocalPlayer.Inventory.Items;
                items.ForEach(_ => ItemPreviewWidget.Instantiate(_, this.transform));
            }
        }
    }
}

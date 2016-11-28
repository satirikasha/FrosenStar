using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemPreviewWidget : UIWidget, ISelectHandler {

    public RectTransform RectTransform { get; private set; }

    public RawImage Icon;
    public Text Name;
    public Image EquipedImage;
    public Color EquipedColor;


    private InventoryItem _Item;

    private SlotItem SlotItem {
        get {
            return _Item as SlotItem;
        }
    }


    public static ItemPreviewWidget Instantiate(InventoryItem item, Transform host) {
        var widget = Instantiate(WidgetResourcesCache.GetWidget<ItemPreviewWidget>());
        widget._Item = item;
        widget.RectTransform = (RectTransform)widget.transform;
        widget.transform.SetParent(host, false);
        widget.Refresh();
        return widget;
    }

    public void Refresh() {
        Icon.texture = _Item.Preview;
        Name.text = _Item.Name;
        if (SlotItem != null) {
            EquipedImage.color = SlotItem.EquipedSlotID >= 0 ? EquipedColor : Color.white;
        }
        else {
            EquipedImage.color = Color.white;
        }
    }

    public void OnSelect(BaseEventData eventData) {
        if (SlotItem != null && SlotItem.GetSlot() != null) {
            ShipPort.Instance.FocusSlot(SlotItem.GetSlot());
        }
    }
}
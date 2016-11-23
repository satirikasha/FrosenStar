using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemPreviewWidget : UIWidget {

    public RectTransform RectTransform { get; private set; }

    public RawImage Icon;
    public Text Name;
    public Image EquipedImage;
    public Color EquipedColor;


    private InventoryItem _Item;

    public static ItemPreviewWidget Instantiate(InventoryItem item, Transform host) {
        var widget = Instantiate(WidgetResourcesCache.GetWidget<ItemPreviewWidget>());
        widget._Item = item;
        widget.RectTransform = (RectTransform)widget.transform;
        widget.transform.SetParent(host, false);
        widget.Refresh();
        return widget;
    }

    public void Refresh() {
        var slotItem = _Item as SlotItem;
        Icon.texture = _Item.Preview;
        Name.text = _Item.Name;
        if (slotItem != null) {
            EquipedImage.color = slotItem.EquipedSlotID >= 0 ? EquipedColor : Color.white;
        }
        else {
            EquipedImage.color = Color.white;
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemPreviewWidget : UIWidget {

    public RectTransform RectTransform { get; private set; }

    public RawImage Icon;
    public Text Name;

    private InventoryItem _Item;

    public static ItemPreviewWidget Instantiate(InventoryItem item, Transform host) {
        var widget = Instantiate(WidgetResourcesCache.GetWidget<ItemPreviewWidget>());
        widget._Item = item;
        widget.Icon.texture = item.Preview;
        widget.Name.text = item.Name;
        widget.RectTransform = (RectTransform)widget.transform;
        widget.transform.SetParent(host, false);
        return widget;
    }
}

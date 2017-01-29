using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SlotListPreviewWidget : ScrollListElementWidget {

    public RawImage Icon;
    public Text Name;
    public Image EquipedImage;
    public Color EquipedColor;

    private ItemSlot _Slot;

    public static SlotListPreviewWidget Instantiate(ItemSlot slot, Transform host) {
        var widget = Instantiate(WidgetResourcesCache.GetWidget<SlotListPreviewWidget>());
        widget._Slot = slot;
        widget.transform.SetParent(host, false);
        widget.Refresh();
        return widget;
    }

    void Awake() {
        this.GetComponent<Button>().onClick.AddListener(() => {
            _Slot.Equip(HangarManager.Instance.SelectedItem as SlotItem);
        });
    }

    public override void Refresh() {
        Name.text = _Slot.name;
    }

    public override void OnSelect(BaseEventData eventData) {
        ShipPort.Instance.FocusSlot(_Slot);
    }
}
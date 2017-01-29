using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Tools;

public class SlotListWidget : ScrollListWidget<SlotListPreviewWidget> {

    public static SlotType CurrentSlotFilter = SlotType.None;

    public InventoryWidgetConfig Config;

    protected override void Start() {
        base.Start();
        _Panel.OnSelected += () => CurrentSlotFilter = Config.UseSlotFilter ? Config.SlotFilter : SlotType.None;
        _Panel.OnDeselected += () => CurrentSlotFilter = SlotType.None;
    }

    public static SlotListWidget Instantiate(InventoryWidgetConfig config) {
        var go = Instantiate(WidgetResourcesCache.GetWidget<SlotListWidget>());
        go.Config = config;
        go.name = GetPanelName(config.SlotFilter);
        return go;
    }

    public override void Refresh() {
        if (_Previews != null) {
            _Previews.ForEach(_ => Destroy(_.gameObject));
        }
        _Previews = new List<SlotListPreviewWidget>();
        if (Config.UseSlotFilter) {
            var items = ShipController.LocalShip.ItemSlots.Where(_ => _.Type == Config.SlotFilter).ToList();
            items.ForEach(_ => RegisterPreview(SlotListPreviewWidget.Instantiate(_, _ScrollRect.content)));
        }
    }

    public static string GetPanelName(SlotType type) {
        return type + "Slots";
    }
}

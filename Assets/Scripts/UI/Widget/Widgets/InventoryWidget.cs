using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Tools;

[Serializable]
public class InventoryWidgetConfig {
    public string Name;
    public bool UseSlotFilter;
    public SlotType SlotFilter;
    public bool CargoOnly;
}

public class InventoryWidget : ScrollListWidget<ItemPreviewWidget> {

    public static SlotType CurrentSlotFilter = SlotType.None;

    public InventoryWidgetConfig Config;

    protected override void Start() {
        base.Start();
        _Panel.OnSelected += () => CurrentSlotFilter = Config.UseSlotFilter ? Config.SlotFilter : SlotType.None;
        _Panel.OnDeselected += () => CurrentSlotFilter = SlotType.None;
    }

    public static InventoryWidget Instantiate(InventoryWidgetConfig config) {
        var go = Instantiate(WidgetResourcesCache.GetWidget<InventoryWidget>());
        go.Config = config;
        go.name = config.Name;
        return go;
    }

    public override void Refresh() {
        if (_Previews != null) {
            _Previews.ForEach(_ => Destroy(_.gameObject));
        }
        _Previews = new List<ItemPreviewWidget>();
        if (Config.UseSlotFilter) {
            var items = PlayerController.LocalPlayer.Inventory.Items.OfType<SlotItem>().Where(_ => _.CheckCompatability(Config.SlotFilter)).ToList();
            items.ForEach(_ => RegisterPreview(ItemPreviewWidget.Instantiate(_, _ScrollRect.content)));
        }
        else {
            if (Config.CargoOnly) {
                var items = PlayerController.LocalPlayer.Inventory.Items.OfType<GenericItem>().ToList();
                items.ForEach(_ => RegisterPreview(ItemPreviewWidget.Instantiate(_, _ScrollRect.content)));
            }
            else {
                var items = PlayerController.LocalPlayer.Inventory.Items;
                items.ForEach(_ => RegisterPreview(ItemPreviewWidget.Instantiate(_, _ScrollRect.content)));
            }
        }
    }
}

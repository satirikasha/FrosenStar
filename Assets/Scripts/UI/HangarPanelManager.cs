using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools;

public class HangarPanelManager : SingletonBehaviour<HangarPanelManager> {

    public float ZOffset = -10;

    public List<InventoryWidgetConfig> InventoryPanels;

    public HangarPanel LastPanel { get; private set; }

    private List<HangarPanel> Panels;

    void Start() {
        InstantiateInventoryPanels();
        Panels = this.GetComponentsInChildren<HangarPanel>(true).ToList();
        Panels.First().Stack = PanelStack.Current;
        Panels.ForEach(_ => {
            _.gameObject.SetActive(true);
            _.transform.localPosition = Vector3.forward * ZOffset;
        });
    }

    public void MoveToPanel(string name) {
        var nextPanel = Panels.FirstOrDefault(_ => _.name == name);
        var curentPanel = Panels.First(_ => _.Stack == PanelStack.Current);

        if (nextPanel != null) {
            if (nextPanel.Stack == PanelStack.Next) {
                curentPanel.Stack = PanelStack.Previous;
            }
            if (nextPanel.Stack == PanelStack.Previous) {
                curentPanel.Stack = PanelStack.Next;
            }

            LastPanel = curentPanel;
            nextPanel.Stack = PanelStack.Current;
        }
    }

    public void MoveToLastPanel() {
        if (LastPanel != null)
            MoveToPanel(LastPanel.name);
    }

    private void InstantiateInventoryPanels() {
        foreach (var config in InventoryPanels) {
            var inventory = InventoryWidget.Instantiate(config);
            inventory.transform.SetParent(this.transform, false);
            if (config.UseSlotFilter) {
                var slot = SlotListWidget.Instantiate(config);
                slot.transform.SetParent(this.transform, false);
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools;

public class HangarPanelManager : SingletonBehaviour<HangarPanelManager> {

    public float ZOffset = -10;

    public List<InventoryWidgetConfig> InventoryPanels;

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
        var panel = Panels.FirstOrDefault(_ => _.name == name);
        if (panel != null) {
            if (panel.Stack == PanelStack.Next) {
                Panels.First(_ => _.Stack == PanelStack.Current).Stack = PanelStack.Previous;
            }
            if(panel.Stack == PanelStack.Previous) {
                Panels.First(_ => _.Stack == PanelStack.Current).Stack = PanelStack.Next;
            }
            panel.Stack = PanelStack.Current;
        }
    }

    private void InstantiateInventoryPanels() {
        foreach (var config in InventoryPanels) {
            var go = InventoryWidget.Instantiate(config);
            go.transform.SetParent(this.transform, false);
        }
    }
}

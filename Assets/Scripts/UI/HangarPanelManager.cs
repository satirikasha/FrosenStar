using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HangarPanelManager : MonoBehaviour {

    private List<HangarPanel> Panels;

    void Awake() {
        Panels = this.GetComponentsInChildren<HangarPanel>(true).ToList();
        Panels.First().Stack = PanelStack.Current;
        Debug.Log(Panels.Count);
        Panels.ForEach(_ => _.gameObject.SetActive(true));
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
}

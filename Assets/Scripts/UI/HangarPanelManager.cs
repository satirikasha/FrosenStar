using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HangarPanelManager : MonoBehaviour {

    private List<CanvasGroup> Panels;

    public CanvasGroup CurrentPanel { get; private set; }
    public List<CanvasGroup> NextPanels { get; private set; }
    public List<CanvasGroup> PreviousPanels { get; private set; }

    void Start() {
        Panels = this.GetComponentsInChildren<CanvasGroup>(true).ToList();
        CurrentPanel = Panels.First();
        Panels.ForEach(_ => _.gameObject.SetActive(_ == CurrentPanel));
    }

    void Update() {

    }

    public void MoveToNext() {

    }

    public void MoveToPrevious() {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManagerNavigationButton : MonoBehaviour {

    public string PanelName;

	void Start () {
        this.GetComponent<Button>().onClick.AddListener(() => HangarPanelManager.Instance.MoveToPanel(PanelName));
	}
}

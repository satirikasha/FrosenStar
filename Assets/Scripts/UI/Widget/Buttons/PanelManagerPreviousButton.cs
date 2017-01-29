using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManagerPreviousButton : MonoBehaviour {

	void Start () {
        this.GetComponent<Button>().onClick.AddListener(() => HangarPanelManager.Instance.MoveToLastPanel());
	}
}

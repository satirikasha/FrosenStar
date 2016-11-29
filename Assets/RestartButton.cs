using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour {

	void Start () {
        this.GetComponent<Button>().onClick.AddListener(() => ApplicationManager.EnterGame());
	}
}

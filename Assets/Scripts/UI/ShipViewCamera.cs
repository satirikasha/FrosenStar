using UnityEngine;
using System.Collections;

public class ShipViewCamera : MonoBehaviour {

    public Transform FocusPoint;

	// Use this for initialization
	void Start () {
        var rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        rt.Create();
        this.GetComponent<Camera>().targetTexture = rt;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShaftMesh : MonoBehaviour {

	public bool Visible { get; private set; }

    public void MarkDirty() {
        Visible = false;
    }

    void OnWillRenderObject() {
        if (Camera.main == Camera.current)
            Visible = true;
    }
}

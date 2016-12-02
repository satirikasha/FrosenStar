using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderDataProvider : MonoBehaviour {

    [Header("Noise")]
    public Texture2D NoiseTex;

    void Start() {
        Shader.SetGlobalTexture("_Noise", NoiseTex);
    }

	void Update () {
        Shader.SetGlobalVector("_CameraPos", Camera.main.transform.position);
        Shader.SetGlobalVector("_CameraForward", Camera.main.transform.forward);
        Shader.SetGlobalVector("_CameraRight", Camera.main.transform.right);
        Shader.SetGlobalVector("_CameraUp", Camera.main.transform.up);
        Shader.SetGlobalFloat("_AspectRatio", (float)Screen.width / (float)Screen.height);
        Shader.SetGlobalFloat("_FieldOfView", Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad * 0.5f) * 2f);
    }
}

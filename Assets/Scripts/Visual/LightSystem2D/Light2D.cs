using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light2D : MonoBehaviour {
    public float Radius = 25;
    public int Resolution = 128;
    private Camera[] _Cameras = new Camera[4];
    private LightShaftMesh[] _LightShaftMeshes = new LightShaftMesh[4];
    private RenderTexture[] _RT = new RenderTexture[4];

    void Start() {
        GenerateCameras();
        GenerateMesh();
    }

    void OnEnable() {
        GenerateRT();
    }

    void OnDisable() {
        CleanupRT();
    }

    void LateUpdate() {
        Render();
    }

    private void GenerateCameras() {
        for (int i = 0; i < 4; i++) {
            var go = new GameObject("CubemapCamera", typeof(Camera));
            go.hideFlags = HideFlags.DontSave;
            go.transform.SetParent(this.transform);
            go.transform.position = transform.position;
            go.transform.rotation = Quaternion.identity * Quaternion.Euler(Vector3.up * 90 * i);
            _Cameras[i] = go.GetComponent<Camera>();
            _Cameras[i].renderingPath = RenderingPath.Forward;
            _Cameras[i].clearFlags = CameraClearFlags.SolidColor;
            _Cameras[i].backgroundColor = Color.black;
            _Cameras[i].SetReplacementShader(Shader.Find("Hidden/DepthSampler"), "RenderType");
            _Cameras[i].farClipPlane = Radius;
            _Cameras[i].targetTexture = _RT[i];
            _Cameras[i].enabled = false;
        }
        RefreshFrustrum();
    }

    private void GenerateMesh() {
        var mesh = new Mesh();
        mesh.SetVertices(new List<Vector3>() { Vector3.zero, Vector3.forward + Vector3.right, Vector3.forward + Vector3.left });
        mesh.SetNormals(new List<Vector3>() { Vector3.up, Vector3.up, Vector3.up });
        mesh.SetUVs(0, new List<Vector2>() { Vector2.right / 2, Vector2.right, Vector2.zero });
        mesh.SetIndices(new int[] { 0, 2, 1 }, MeshTopology.Triangles, 0);
        mesh.name = "RenderQuad";
        for (int i = 0; i < 4; i++) {
            var go = new GameObject("RenderQuad", typeof(MeshFilter), typeof(MeshRenderer), typeof(LightShaftMesh));
            go.hideFlags = HideFlags.DontSave;
            go.transform.position = transform.position;
            go.transform.rotation = Quaternion.identity * Quaternion.Euler(Vector3.up * 90 * i);
            go.transform.localScale = Vector3.one * Radius;
            go.transform.SetParent(this.transform);
            var meshRenderer = go.GetComponent<MeshRenderer>();
            var meshFilter = go.GetComponent<MeshFilter>();
            var material = new Material(Shader.Find("Unlit/Texture"));
            material.SetTexture("_MainTex", _RT[i]);
            meshRenderer.material = material;
            meshFilter.sharedMesh = mesh;
            _LightShaftMeshes[i] = go.GetComponent<LightShaftMesh>();
        }
    }

    private void GenerateRT() {
        for (int i = 0; i < 4; i++) {
            _RT[i] = RenderTexture.GetTemporary(Resolution, 1, 32);
        }
    }

    private void CleanupRT() {
        for (int i = 0; i < 4; i++) {
            RenderTexture.ReleaseTemporary(_RT[i]);
        }
    }

    private void RefreshFrustrum() {
        var radAngle = 90 * Mathf.Deg2Rad;
        var radHFOV = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) / Resolution);
        var fov = Mathf.Rad2Deg * radHFOV;
        for (int i = 0; i < 4; i++) {
            _Cameras[i].fieldOfView = fov;
        }
    }

    private void Render() {
        for (int i = 0; i < 4; i++) {
            if (_LightShaftMeshes[i].Visible)
                _Cameras[i].Render();
            _LightShaftMeshes[i].MarkDirty();
        }
    }
}

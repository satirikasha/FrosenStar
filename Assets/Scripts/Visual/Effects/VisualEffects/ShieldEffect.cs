using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : MonoBehaviour {

    public float ImpactCooldown;

    public float Intensity { get; set; }

    private Material _ShieldMaterial;
    private List<Vector4> _Impacts = new List<Vector4>();

    void Awake() {
        var renderer = this.GetComponent<MeshRenderer>();
        _ShieldMaterial = new Material(renderer.sharedMaterial);
        renderer.material = _ShieldMaterial;
        _ShieldMaterial.SetFloat("_ImpactCooldown", ImpactCooldown);
    }


    void Update() {
        UpdateImpacts();
    }

    private void UpdateImpacts() {
        _Impacts.ForEach(_ => _.z -= Time.deltaTime);
        _Impacts.RemoveAll(_ => _.z < 0);
        _ShieldMaterial.SetVectorArray("_Impacts", _Impacts);
    }

    public void AddImpact(Vector3 position) {
        _Impacts.Add(new Vector4(position.x, position.y, position.z, ImpactCooldown));
    }
}

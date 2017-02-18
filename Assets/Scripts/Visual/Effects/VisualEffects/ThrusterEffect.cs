using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterEffect : VisualEffect {

    public float Intensity { get; set; }

    private ParticleSystem _Thruster;
    private Light _Light;
    private float _MaxLightIntensity;

    void Awake() {
        _Thruster = this.GetComponent<ParticleSystem>();
        _Light = this.GetComponent<Light>();
        _MaxLightIntensity = _Light.intensity;
    }

    void FixedUpdate() {
        _Light.intensity = _MaxLightIntensity * Intensity;
        _Light.enabled = Intensity > 0;
        _Thruster.startColor = new Color(1, 1, 1, Intensity);
        _Thruster.enableEmission = Intensity > 0;
        _Thruster.Simulate(Time.fixedDeltaTime, true, false);
    }
}

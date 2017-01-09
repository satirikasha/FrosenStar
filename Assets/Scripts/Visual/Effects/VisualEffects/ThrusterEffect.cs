using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterEffect : VisualEffect {

    public float Intensity { get; set; }

    private ParticleSystem _Thruster;

    protected override void Awake() {
        base.Awake();
        _Thruster = this.GetComponent<ParticleSystem>();
    }

    void FixedUpdate() {
        _Thruster.startColor = new Color(1, 1, 1, Intensity);
        //_Thruster.enableEmission = Intensity > 0;
        _Thruster.Simulate(Time.fixedDeltaTime, true, false);
    }
}

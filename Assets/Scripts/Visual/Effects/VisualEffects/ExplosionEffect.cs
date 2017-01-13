using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : VisualEffect {

    public bool Test;

    public float Duration = 1;
    public AnimationCurve IntensityMap = AnimationCurve.EaseInOut(0, 0, 1, 1);

    protected override IEnumerator PlayTask() {
        var timeLeft = Duration;
        var light = this.GetComponent<Light>();
        var intensity = 2;

        foreach(var ps in this.GetComponentsInChildren<ParticleSystem>()) {
            ps.Play();
        }

        while( timeLeft > 0) {
            light.intensity = intensity * IntensityMap.Evaluate(1 - (timeLeft / Duration));
            yield return null;
            timeLeft -= Time.deltaTime;
        }

        light.intensity = 0;
        this.gameObject.SetActive(false);
    }

    void OnValidate() {
        if (Test) {
            Test = false;
            Play();
        }
    }
}

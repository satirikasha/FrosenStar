using UnityEngine;
using System.Collections;

public class Engine : ShipPart {

    public float EnergyConsumption = 0.25f;

    public float Thrust = 750;

    private ThrusterEffect _ThruserEffect;

    void Awake() {
        _ThruserEffect = Instantiate(VisualEffect.GetEffectResource<ThrusterEffect>(), this.transform, false);
    }

    public void ApplyThrust(Rigidbody rigidbody, float throttle, float deltaTime) {
        //if (Input.GetKey(KeyCode.LeftShift))
        //    throttle *= 2.5f;

        if (throttle < 0)
            throttle *= 0.25f;

        if (Ship.ConsumeEnergy(Mathf.Abs(throttle) * deltaTime * EnergyConsumption)) {
            // WARNING: Forward vector depends on a model (in this case "-this.transform.right")
            rigidbody.AddForceAtPosition(this.transform.forward * throttle * Thrust, this.transform.position);
            _ThruserEffect.Intensity = throttle;
        }
    }
}

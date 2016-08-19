using UnityEngine;
using System.Collections;

public class Engine : ShipItem {

    public float Thrust = 750;

    public void ApplyThrust(Rigidbody rigidbody, float throttle) {
        rigidbody.AddForceAtPosition(this.transform.forward * throttle * Thrust, this.transform.position);
    }
}

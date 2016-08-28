using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public static PlayerController LocalPlayer { get; private set; }

    public ShipController Ship { get; private set; }

    public Vector3 Velocity {
        get {
            return Ship.Velocity;
        }
    }

    public Vector3 Position {
        get {
            return Ship.Position;
        }
    }

    void Awake() {
        LocalPlayer = this;
        Ship = this.GetComponent<ShipController>();
    }
}

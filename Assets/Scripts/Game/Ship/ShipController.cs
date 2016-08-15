using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ShipController : MonoBehaviour {

    private const float BodyRollCoeff = 0.5f;

    public Vector3 Velocity {
        get
        {
            return _Rigidbody.velocity;
        }
    }

    public float EngineForce = 750;
    public float TurnTorque = 150;
    public float StabilizationTorque = 25;
    public float StabilizationScaleLon = 1;
    public float StabilizationScaleLat = 5;

    public List<ItemSlot> ItemSlots { get; private set; }
    public List<Weapon> Weapons{ get; private set; }
    public List<Engine> Engines { get; private set; }

    private Rigidbody _Rigidbody;

    void Awake() {
        _Rigidbody = this.GetComponent<Rigidbody>();        
    }

    void Start() {
        RefreshSlots();
    }

    void FixedUpdate() {
        UpdateEngineForce();
        UpdateRotationTorque();
        UpdateStabilizationTorque();
    }

    void Update() {
        UpdateFire();
    }

    private void UpdateEngineForce() {
        var vertical = Input.GetAxis("Vertical");
        _Rigidbody.AddForce(this.transform.forward * vertical * EngineForce);
    }

    private void UpdateRotationTorque() {
        var horizontal = Input.GetAxis("Horizontal");
        var turnTorque = this.transform.up * horizontal * TurnTorque;
        var rollTorque = this.transform.forward * horizontal * TurnTorque * BodyRollCoeff * -1;
        _Rigidbody.AddTorque(turnTorque + rollTorque);
    }

    private void UpdateStabilizationTorque() {
        var lonRotation = this.transform.rotation.eulerAngles.z;
        var latRotation = this.transform.rotation.eulerAngles.x;
        var lonOffset = lonRotation > 180 ? lonRotation - 360 : lonRotation;
        var latOffset = latRotation > 180 ? latRotation - 360 : latRotation;
        var lonStabilization = -lonOffset * StabilizationTorque * StabilizationScaleLon * this.transform.forward;
        var latStabilization = -latOffset * StabilizationTorque * StabilizationScaleLat * this.transform.right;
        _Rigidbody.AddTorque(lonStabilization + latStabilization);
    }

    private void UpdateFire() {
        if (Input.GetButtonDown("Fire")) {
            Weapons.ForEach(_ => _.StartFire());
        }
        if (Input.GetButtonUp("Fire")) {
            Weapons.ForEach(_ => _.StopFire());
        }
    }

    public void RefreshSlots() {
        ItemSlots = this.GetComponentsInChildren<ItemSlot>().ToList();
        Weapons = ItemSlots.Select(_ => _.Item).OfType<Weapon>().ToList();
        Engines = ItemSlots.Select(_ => _.Item).OfType<Engine>().ToList();
    }
}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ShipController : MonoBehaviour {

    private const float BodyRollCoeff = 0.5f;

    public Vector3 Velocity {
        get
        {
            return Rigidbody.velocity;
        }
    }

    public Vector3 Position {
        get
        {
            return Rigidbody.position;
        }
    }

    public float Health { get; private set; }
    public float NormalizedHealth {
        get
        {
            return Health / MaxHealth;
        }
    }
    public float MaxHealth = 100;

    public float Energy { get; private set; }
    public float NormalizedEnergy {
        get
        {
            return Energy / MaxEnergy;
        }
    }
    public float MaxEnergy = 100;

    public float TurnTorque = 150;
    public float StabilizationTorque = 2.5f;
    public float StabilizationScaleLon = 1;
    public float StabilizationScaleLat = 5;

    public Rigidbody Rigidbody { get; private set; }
    public List<ItemSlot> ItemSlots { get; private set; }
    public List<Weapon> Weapons { get; private set; }
    public List<Engine> Engines { get; private set; }


    void Awake() {
        Rigidbody = this.GetComponent<Rigidbody>();
        Health = MaxHealth;
        Energy = MaxEnergy;
    }

    void Start() {
        RefreshSlots();
    }

    void FixedUpdate() {
        UpdateEngines();
        UpdateRotationTorque();
        UpdateStabilizationTorque();
    }

    void Update() {
        UpdateWeapons();
    }

    public void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.impulse.magnitude);
        Health -= collision.impulse.magnitude / 5;
    }

    public void OnCollisionStay(Collision collision) {
        Debug.Log(collision.impulse.magnitude);
        Health -= collision.impulse.magnitude / 5;
    }

    private void UpdateEngines() {
        if (ApplicationManager.GameMode) {
            var vertical = Input.GetAxis("Vertical");
            Engines.ForEach(_ => _.ApplyThrust(Rigidbody, vertical, Time.fixedDeltaTime));
        }
    }

    private void UpdateRotationTorque() {
        if (ApplicationManager.GameMode) {
            var horizontal = Input.GetAxis("Horizontal");
            var turnTorque = this.transform.up * horizontal * TurnTorque;
            var rollTorque = this.transform.forward * horizontal * TurnTorque * BodyRollCoeff * -1;
            Rigidbody.AddTorque(turnTorque + rollTorque);
        }
    }

    private void UpdateStabilizationTorque() {
        if (ApplicationManager.GameMode) {
            var lonRotation = this.transform.rotation.eulerAngles.z;
            var latRotation = this.transform.rotation.eulerAngles.x;
            var lonOffset = lonRotation > 180 ? lonRotation - 360 : lonRotation;
            var latOffset = latRotation > 180 ? latRotation - 360 : latRotation;
            var lonStabilization = -lonOffset * StabilizationTorque * StabilizationScaleLon * this.transform.forward;
            var latStabilization = -latOffset * StabilizationTorque * StabilizationScaleLat * this.transform.right;
            Rigidbody.AddTorque(lonStabilization + latStabilization);
        }
    }

    private void UpdateWeapons() {
        if (ApplicationManager.GameMode) {
            if (Input.GetButtonDown("Fire") && !GameManager.Instance.Paused) {
                Weapons.ForEach(_ => _.StartFire());
            }
            if (Input.GetButtonUp("Fire")) {
                Weapons.ForEach(_ => _.StopFire());
            }
        }
    }

    public void RefreshSlots() {
        ItemSlots = this.GetComponentsInChildren<ItemSlot>().ToList();
        Weapons = ItemSlots.Select(_ => _.Item).OfType<Weapon>().ToList();
        Engines = ItemSlots.Select(_ => _.Item).OfType<Engine>().ToList();
    }

    /// <summary>
    /// Perform energy consumption
    /// </summary>
    /// <param name="energy">Ammount of energy to consume</param>
    /// <returns>Transaction successful</returns>
    public bool ConsumeEnergy(float energy) {
        if(Energy >= energy) {
            Energy -= energy;
            return true;
        }
        return false;
    }

    public void RestoreEnergy(float energy) {
        Energy = Mathf.Clamp(Energy + energy, 0, MaxEnergy);
    }
}

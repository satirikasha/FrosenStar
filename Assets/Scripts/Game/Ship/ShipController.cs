﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools.Damage;
using System;
using Tools.EQS;

public class ShipController : InitializedBehaviour, IDamagable {

    private const float BodyRollCoeff = 0.5f;
    private const float CollisionDamageCoeff = 0.2f;

    public event Action OnBecameDead;

    public float findRadius = 3;
    public float looseRadius = 6;
    public float dotFactor = 0.5f;

    public ShipItem Item;

    public static ShipController LocalShip {
        get {
            return PlayerController.LocalPlayer.Ship;
        }
    }

    public bool IsLocalShip {
        get {
            return this == LocalShip;
        }
    }

    public Vector3 Velocity {
        get {
            return Rigidbody.velocity;
        }
    }

    public Vector3 Position {
        get {
            return Rigidbody.position;
        }
    }

    public float Health { get; private set; }
    public float NormalizedHealth {
        get {
            return Health / Item.Health;
        }
    }

    public float Energy { get; private set; }
    public float NormalizedEnergy {
        get {
            return Energy / Item.Energy;
        }
    }

    public float StabilizationTorque = 2.5f;
    public float StabilizationScaleLon = 1;
    public float StabilizationScaleLat = 5;

    public Rigidbody Rigidbody { get; private set; }
    public Inventory Inventory { get; private set; }
    public Collider Collider { get; private set; }
    public float Width { get; private set; }


    public List<ItemSlot> ItemSlots { get; private set; }
    public List<Weapon> Weapons { get; private set; }
    public List<Engine> Engines { get; private set; }

    public float Throttle { get; private set; }
    public float Steering { get; private set; }

    void Awake() {
        Init();
    }

    void Start() {
        Health = Item.Health;
        Energy = Item.Energy;
        Rigidbody.mass = Item.Mass;
        Rigidbody.drag = Item.Mass * 0.005f;
        Rigidbody.angularDrag = Item.Mass * 0.025f;
        RefreshSlots();
    }

    void FixedUpdate() {
        UpdateEngines();
        UpdateRotationTorque();
        UpdateStabilizationTorque();
        UpdateFocus();
    }

    void Update() {

    }

    public void OnCollisionEnter(Collision collision) {
        ApplyCollisionDamage(collision);
    }

    public void OnCollisionStay(Collision collision) {
        ApplyCollisionDamage(collision);
    }

    protected override void Init() {
        Rigidbody = this.GetComponent<Rigidbody>();
        Inventory = this.GetComponent<Inventory>();
        Collider = this.GetComponentInChildren<Collider>();
        Width = (Collider as MeshCollider).sharedMesh.bounds.extents.x;
    }

    private void UpdateEngines() {
        if (ApplicationManager.GameMode) {
            Engines.ForEach(_ => _.ApplyThrust(Rigidbody, Throttle, Time.fixedDeltaTime));
        }
    }

    private void UpdateRotationTorque() {
        if (ApplicationManager.GameMode) {
            var turnTorque = this.transform.up * Steering * Item.Handling;
            var rollTorque = this.transform.forward * Steering * Item.Handling * BodyRollCoeff * -1;
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

    private void UpdateFocus() {
        var item = EQS
            .GetItems<FocusedItem>(findRadius)
            .Where(_ => Vector3.Dot((_.transform.position - this.transform.position).normalized, this.transform.forward) > dotFactor)
            .OrderBy(_ => {
                var delta = _.transform.position - this.transform.position;
                return Vector3.Dot(delta.normalized, this.transform.forward) - delta.sqrMagnitude;
            })
            .FirstOrDefault();

        FocusedItem.CurrentLookAtItem = item;
        if (item != null) {
            item.ApplyFocus(2 * Time.deltaTime);
        }
    }

    public void StartFire() {
    if (ApplicationManager.GameMode && !GameManager.Instance.Paused) {
        Weapons.ForEach(_ => _.StartFire());
    }
    }

    public void StopFire() {
    if (ApplicationManager.GameMode) {
        Weapons.ForEach(_ => _.StopFire());
    }
    }

    public void SetThrottle(float value) {
        Throttle = Mathf.Clamp(value, -1, 1);
    }

    public void SetSteering(float value) {
        Steering = Mathf.Clamp(value, -1, 1);
    }

    public void RefreshSlots() {
        ItemSlots = this.GetComponentsInChildren<ItemSlot>().ToList();
        Weapons = ItemSlots.Select(_ => _.ShipPart).OfType<Weapon>().ToList();
        Engines = ItemSlots.Select(_ => _.ShipPart).OfType<Engine>().ToList();
    }

    public void ApplyCollisionDamage(Collision collision) {
        ApplyDamage(
            new Damage() {
                Ammount = collision.impulse.magnitude * CollisionDamageCoeff,
                Instigator = collision.gameObject,
                Source = collision.gameObject,
                Type = DamageType.Physical
            });
    }

    public void ApplyDamage(Damage damage) {
        Health -= damage.Ammount;
        if (Health <= 0) {
            Destroy();
        }
    }

    public void Destroy() {
        this.gameObject.SetActive(false);
        if (OnBecameDead != null)
            OnBecameDead();
    }

    public void SetHealth(float health) {
        Health = Mathf.Clamp(health, 0, Item.Health);
    }

    public bool ConsumeEnergy(float energy) {
        if (Energy >= energy) {
            Energy -= energy;
            return true;
        }
        return false;
    }

    public void RestoreEnergy(float energy) {
        Energy = Mathf.Clamp(Energy + energy, 0, Item.Energy);
    }

    public void SetEnergy(float energy) {
        Energy = Mathf.Clamp(energy, 0, Item.Energy);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools.Damage;

public class ShipController : MonoBehaviour, IDamagable {

    private const float BodyRollCoeff = 0.5f;
    private const float CollisionDamageCoeff = 0.2f;

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

    public ShipItem Item;

    public float Health { get; private set; }
    public float NormalizedHealth {
        get
        {
            return Health / Item.Health;
        }
    }

    public float Energy { get; private set; }
    public float NormalizedEnergy {
        get
        {
            return Energy / Item.Energy;
        }
    }

    public float StabilizationTorque = 2.5f;
    public float StabilizationScaleLon = 1;
    public float StabilizationScaleLat = 5;

    public Rigidbody Rigidbody { get; private set; }
    public Inventory Inventory { get; private set; }

    public List<ItemSlot> ItemSlots { get; private set; }
    public List<Weapon> Weapons { get; private set; }
    public List<Engine> Engines { get; private set; }


    void Awake() {
        Rigidbody = this.GetComponent<Rigidbody>();
        Inventory = this.GetComponent<Inventory>();
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
    }

    void Update() {
        UpdateWeapons();
    }

    public void OnCollisionEnter(Collision collision) {
        ApplyCollisionDamage(collision);
    }

    public void OnCollisionStay(Collision collision) {
        ApplyCollisionDamage(collision);
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
            var turnTorque = this.transform.up * horizontal * Item.Handling;
            var rollTorque = this.transform.forward * horizontal * Item.Handling * BodyRollCoeff * -1;
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
        Health = Mathf.Max(0, Health - damage.Ammount);
    }

    public void SetHealth(float health) {
        Health = Mathf.Clamp(health, 0, Item.Health);
    }

    public bool ConsumeEnergy(float energy) {
        if(Energy >= energy) {
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

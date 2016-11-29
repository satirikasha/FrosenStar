using UnityEngine;
using System.Collections;

public abstract class Weapon : ShipPart {

    public float EnergyConsumption = 0.5f;

    public float Cooldown = 0.15f;

    public bool Recharged { get; protected set; }

    public WeaponItem WeaponItem {
        get {
            return (WeaponItem)Item;
        }
    }

    private bool _IsFiring;

    protected virtual void Awake() {
        Recharged = true;
    }

    protected virtual void Update() {
        if (_IsFiring)
            Fire();
    }

    public virtual void StartFire() {
        _IsFiring = true;
    }

    public virtual void StopFire() {
        _IsFiring = false;
    }

    protected virtual void Fire() {
        if (Recharged && Ship.ConsumeEnergy(EnergyConsumption)) {
            Recharged = false;
            PerformShot();
            Recharge();
        }
    }

    protected abstract void PerformShot();

    protected void Recharge() {
        StartCoroutine(RechargeTask());
    }

    private IEnumerator RechargeTask() {
        yield return new WaitForSeconds(Cooldown);
        Recharged = true;
    }
}

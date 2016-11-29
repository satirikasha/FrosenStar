using System;
using System.Collections;
using System.Collections.Generic;
using Tools.Damage;
using UnityEngine;

public class Turret : Weapon, IDamagable {

    public float Damage = 15;

    public Projectile Projectile;

    public Transform[] FirePivots;

    public float Health = 100;

    public float RotationSpeed = 25;

    protected override void Update() {
        base.Update();
        if (CheckVision()) {
            this.transform.rotation = Quaternion.RotateTowards(
                this.transform.rotation,
                Quaternion.LookRotation(PlayerController.LocalPlayer.transform.position - this.transform.position),
                RotationSpeed * Time.deltaTime
                );

            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, this.transform.forward, out hit) && hit.transform == PlayerController.LocalPlayer.transform) {
                StartFire();
            }
            else {
                StopFire();
            }
        }
        else {
            StopFire();
        }
    }

    private bool CheckVision() {
        RaycastHit hit;
        return Physics.Raycast(
            this.transform.position,
            PlayerController.LocalPlayer.transform.position - this.transform.position,
            out hit) &&
            hit.transform == PlayerController.LocalPlayer.transform;
    }

    protected override void PerformShot() {
        foreach (var firePivot in FirePivots) {
            var projectile = Instantiate<Projectile>(Projectile);
            projectile.transform.position = firePivot.transform.position;
            projectile.transform.rotation = firePivot.transform.rotation;
            projectile.Damage = new Damage() {
                Ammount = Damage,
                Instigator = this.gameObject,
                Source = projectile.gameObject,
                Type = DamageType.Impact
            };
        }
    }

    protected override void Fire() {
        if (Recharged) {
            Recharged = false;
            PerformShot();
            Recharge();
        }
    }

    public void ApplyDamage(Damage damage) {
        Health -= damage.Ammount;
        if (Health <= 0)
            Destroy(this.gameObject);
    }
}

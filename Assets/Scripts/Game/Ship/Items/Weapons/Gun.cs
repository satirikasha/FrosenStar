using System;
using UnityEngine;

public class Gun : Weapon {

    public Projectile Projectile;

    public Transform FirePivot;

    public override void Awake() {
        base.Awake();
    }

    protected override void PerformShot() {
        var projectile = Instantiate<Projectile>(Projectile);
        projectile.transform.position = FirePivot.transform.position;
        projectile.transform.rotation = FirePivot.transform.rotation;
        projectile.InheritedSpeed = Vector3.Dot(Ship.Velocity, FirePivot.forward);
    }
}

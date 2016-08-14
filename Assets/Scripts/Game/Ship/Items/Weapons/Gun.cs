using System;
using UnityEngine;

public class Gun : Weapon {

    public Projectile Projectile;

    public Transform FirePivot;

    protected override void PerformShot() {
        var projectile = Instantiate<Projectile>(Projectile);
        projectile.transform.position = FirePivot.transform.position;
        projectile.transform.rotation = FirePivot.transform.rotation;
    }
}

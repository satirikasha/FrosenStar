using System;
using Tools.Damage;
using UnityEngine;

public class Gun : Weapon {

    public Projectile Projectile;

    public Transform FirePivot;

    protected override void PerformShot() {
        var projectile = Instantiate<Projectile>(Projectile);
        projectile.transform.position = FirePivot.transform.position;
        projectile.transform.rotation = FirePivot.transform.rotation;
        projectile.InheritedSpeed = Vector3.Dot(Ship.Velocity, FirePivot.forward);
        projectile.Damage = new Damage() {
            Ammount = WeaponItem.Damage,
            Instigator = Ship.gameObject,
            Source = projectile.gameObject,
            Type = DamageType.Impact
        };
    }
}

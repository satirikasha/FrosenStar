﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CinematicEffects;
using Tools;
using Tools.Damage;

public class Projectile : MonoBehaviour {
    public float InitialSpeed = 3;
    [Interval(0, 10)]
    public Vector2 LifespanRange = new Vector2(0.5f, 1.5f);
    
    public float InheritedSpeed { get; set; }
    public Damage Damage { get; set; }

    public float Lifespan { get; private set; }
    public float LifeTime { get; private set; }

    void Awake() {
        Lifespan = LifespanRange.GetRandomValue();
        LifeTime = Lifespan;
    }

    void FixedUpdate() {
        var deltaTime = Time.fixedDeltaTime;
        this.transform.Translate(Vector3.forward * (InitialSpeed + InheritedSpeed) * deltaTime);
        this.transform.position = Vector3.Scale(this.transform.position, new Vector3(1, 0, 1));
        LifeTime -= deltaTime;
        if (LifeTime <= 0)
            Destroy(this.gameObject);
    }

    public void OnTriggerEnter(Collider other) {
        if (Damage.Instigator != null && other.transform.IsChildOf(Damage.Instigator.transform)) // Ignore the case of self hit
            return;
        var damagable = other.gameObject.GetComponent<IDamagable>();
        if (damagable != null) {
            damagable.ApplyDamage(Damage);
        }

        var effect = VisualEffect.GetEffect<ExplosionEffect>("Explosion");
        effect.transform.position = this.transform.position;
        effect.Play();
        Destroy(this.gameObject);
    }
}

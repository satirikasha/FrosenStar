using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    public float InitialSpeed = 3;
    public float Lifespan = 5;

    public float LifeTime { get; private set; }

    void Awake() {
        LifeTime = Lifespan;
    }

    void Update() {
        var deltaTime = Time.deltaTime;
        this.transform.Translate(Vector3.forward * InitialSpeed * deltaTime);
        LifeTime -= deltaTime;
        if (LifeTime <= 0)
            Destroy(this.gameObject);
    }
}

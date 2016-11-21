using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class HangarPlayerStart : PlayerStart {

    public float Scale = 4;

    public override ShipController SpawnShip(ShipItem ship) {
        var go = base.SpawnShip(ship);
        go.transform.localScale = Vector3.one * Scale;
        return go;
    }

    public override void OnValidate() {
        // Shouldn't snap to zero plane
    }
}

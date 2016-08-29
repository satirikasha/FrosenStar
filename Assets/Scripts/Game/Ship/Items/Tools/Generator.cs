using UnityEngine;
using System.Collections;

public class Generator : ShipItem {

    public float EnergyRegeneration = 0.5f;

    public void Update() {
        Ship.RestoreEnergy(EnergyRegeneration * Time.deltaTime);
    }
}

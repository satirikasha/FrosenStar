using UnityEngine;
using System.Collections;

public class DefaultPlayerStart : PlayerStart {
    public ShipItemTemplate DefaultShip;

    void Awake() {
        if (ApplicationManager.NewGame)
            SpawnShip(DefaultShip.GenerateItem() as ShipItem);
    }
}
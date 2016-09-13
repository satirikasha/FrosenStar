using UnityEngine;
using System.Collections;

public class DefaultPlayerStart : PlayerStart {
    public ShipItemTemplate DefaultShip;

    void Awake() {
        SpawnPlayer(DefaultShip.GenerateItem() as ShipItem);
    }
}
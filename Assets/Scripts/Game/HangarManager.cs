using UnityEngine;
using System.Collections;
using System.Linq;

public class HangarManager : MonoBehaviour {

    void Start() {
        var shipItem = GameData.Current.PlayerData.ShipData.ShipItem;
        PlayerStart.GetAvailable().First().SpawnShip(shipItem);
    }

    void Update() {

    }
}

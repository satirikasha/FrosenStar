using UnityEngine;
using System.Collections;
using System.Linq;

public class HangarManager : MonoBehaviour {

    void Awake() {
        Cursor.lockState = CursorLockMode.Locked;

        var shipItem = GameData.Current.PlayerData.ShipData.ShipItem;
        PlayerStart.GetAvailable().First().SpawnShip(shipItem);
    }

    void Update() {

    }

    public void LeaveHangar() {
        ApplicationManager.EnterGame();
    }
}

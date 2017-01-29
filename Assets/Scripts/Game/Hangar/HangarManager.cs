using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using Tools;

public class HangarManager : SingletonBehaviour<HangarManager> {

    public InventoryItem SelectedItem { get; private set; }
    public event Action<InventoryItem> OnSelectedItemChanged;

    protected override void Awake() {
        base.Awake();
        //Cursor.lockState = CursorLockMode.Locked;

        var shipItem = GameData.Current.PlayerData.ShipData.ShipItem;
        PlayerStart.GetAvailable().First().SpawnShip(shipItem);
    }

    void Update() {

    }

    public void SetSelectedItem(InventoryItem item) {
        if(SelectedItem != item) {
            SelectedItem = item;
            if (OnSelectedItemChanged != null)
                OnSelectedItemChanged(item);
        }
    }

    public void LeaveHangar() {
        ApplicationManager.EnterGame();
    }
}

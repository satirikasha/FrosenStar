using UnityEngine;
using System.Collections;
using Tools;
using System.Linq;

public class GameManager : SingletonBehaviour<GameManager> {

    public bool Paused {
        get
        {
            return UIManager.CurrentPanel == UIManager.GetPanel("InventoryPanel");
        }
    }

    public ShipItemTemplate DefaultShip;

    protected override void Awake() {
        base.Awake();

        Time.timeScale = 1;

        if (ApplicationManager.NewGame) {
            var shipItem = (ShipItem)DefaultShip.GenerateItem();
            PlayerStart.GetDefault().SpawnShip(shipItem);
        }
        else {
            GameData.Load();
            var shipItem = GameData.Current.PlayerData.ShipData.ShipItem;
            PlayerStart.GetAvailable().First().SpawnShip(shipItem);
        }
       
    }

    void Update () {
        UpdateInput();
	}

    public void UpdateInput() {
        if (Input.GetKeyUp(KeyCode.K))
            GameData.Save();
        if (Input.GetKeyUp(KeyCode.L))
            GameData.Load();


        if (Input.GetKeyUp(KeyCode.I)) {
            var inventoryPanel = UIManager.GetPanel("InventoryPanel");
            if (!Paused) {
                UIManager.SetCurrentPanel(inventoryPanel, customAction: _ => Time.timeScale = 1 - _);
            }
            else {
                if (UIManager.CurrentPanel == inventoryPanel) {
                    UIManager.SetCurrentPanel(UIManager.GetPanel("GamePanel"), customAction: _ => Time.timeScale = _);
                }
            }
        }
    }

    public static void TogglePause() {

    }

    public static void Pause() {

    }

    public static void Resume() {

    }
}

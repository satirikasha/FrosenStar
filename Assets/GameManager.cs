using UnityEngine;
using System.Collections;

public class GameManager : SingletonBehaviour<GameManager> {

    public bool Paused { get; private set; }

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update () {
        UpdateInput();
	}

    public void UpdateInput() {
        if (Input.GetKeyUp(KeyCode.I)) {
            var inventoryPanel = UIManager.GetPanel("InventoryPanel");
            if (!Paused) {
                UIManager.SetCurrentPanel(inventoryPanel, customAction: _ => Time.timeScale = 1 - _);
                Paused = true;
            }
            else {
                if (UIManager.CurrentPanel == inventoryPanel) {
                    UIManager.SetCurrentPanel(UIManager.GetPanel("GamePanel"), customAction: _ => Time.timeScale = _);
                    Paused = false;
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

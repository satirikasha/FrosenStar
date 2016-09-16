using UnityEngine;
using System.Collections;
using Tools.EQS;
using System.Linq;

public class PlayerController : MonoBehaviour {

    public static PlayerController LocalPlayer { get; private set; }

    public EQSItem SelectedItem;

    public ShipController Ship { get; private set; }
    public Inventory Inventory { get; private set; }

    public Vector3 Velocity {
        get {
            return Ship.Velocity;
        }
    }

    public Vector3 Position {
        get {
            return Ship.Position;
        }
    }

    public bool IsLocalPlayer {
        get {
            return this == LocalPlayer;
        }
    }

    void Awake() {
        if (this.GetComponent<PlayerController>())
            LocalPlayer = this;
        Ship = this.GetComponent<ShipController>();
        Inventory = this.GetComponent<Inventory>();

        if (IsLocalPlayer && !ApplicationManager.NewGame)
            Load();
    }

    void Update() {
        SelectedItem = EQS.GetItems(5).OrderBy(_ => Vector3.Dot(-_.Delta.normalized, this.transform.forward)).FirstOrDefault();
    }

    private void Load() {
        Inventory.Items.Clear();
        Inventory.AddItems(GameData.Current.PlayerData.ShipData.InventoryData.InventoryItems, Inventory);
    }
}

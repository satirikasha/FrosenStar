using UnityEngine;
using System.Collections;
using Tools;
using Tools.EQS;
using System.Linq;

/// <summary>
/// This class represents a player, who owns a ship
/// </summary>
public class PlayerController : SingletonBehaviour<PlayerController> {

    public static PlayerController LocalPlayer {
        get {
            return Instance;
        }
    }

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

    protected override void Awake() {
        base.Awake();
        Ship = this.GetComponent<ShipController>();
        Inventory = this.GetComponent<Inventory>();

        if (ApplicationManager.NewGame) {
            Construct();
        }
        else {
            Load();
        }
    }

    void Update() {
        SelectedItem = EQS.GetItems(5).OrderBy(_ => Vector3.Dot(-_.Delta.normalized, this.transform.forward)).FirstOrDefault();
    }

    private void Construct() {
        Ship.RefreshSlots();
        Ship.ItemSlots.ForEach(_ => _.Construct());
    }

    private void Load() {
        Ship.RefreshSlots();
        Inventory.Items.Clear();
        Inventory.AddItems(GameData.Current.PlayerData.InventoryData.InventoryItems, Inventory);
        Inventory.Items.OfType<SlotItem>().ForEach(_ => {
            if (_.EquipedSlotID >= 0) {
                var slot = Ship.ItemSlots.FirstOrDefault(s => s.ID == _.EquipedSlotID);
                if(slot != null) {
                    slot.Equip(_);
                }
            }
        });
    }
}

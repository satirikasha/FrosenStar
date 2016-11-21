using UnityEngine;
using System.Collections;
using System;
using Tools.UI.Markers;

namespace UI.Markers {


    public class SlotMarkerProvider : MarkerProvider<SlotMarkerWidget, SlotMarkerData> {

        private ItemSlot _Slot;

        void Start() {
            _Slot = this.GetComponent<ItemSlot>();
        }

        public override bool GetVisibility() {
            if (ApplicationManager.GameMode)
                return false;
            return _Slot.Type == InventoryWidget.CurrentSlotFilter;
            //var cam2Ship = PlayerController.LocalPlayer.Position - Camera.main.transform.position;
            //var soc2Ship = PlayerController.LocalPlayer.Position - this.transform.position;
            //return Vector3.Dot(cam2Ship, soc2Ship) > 0;
        }

        public override void UpdateData() {
            Data.WorldPosition = this.transform.position;
            Data.Preview = _Slot.ShipPart.Item.Preview;
        }
    }

    public class SlotMarkerData : MarkerData {
        public Texture2D Preview;
    }
}
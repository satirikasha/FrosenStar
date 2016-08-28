using UnityEngine;
using System.Collections;
using System;

namespace UI.Markers {


    public class SlotMarkerProvider : MarkerProvider {

        public override Type RequiredMarkerType {
            get {
                return typeof(SlotMarkerWidget);
            }
        }

        public override MarkerData GetMarkerData() {
            var data = new SlotMarkerData();
            data.WorldPosition = this.transform.position;
            return data;
        }

        public override bool GetVisibility() {
            var cam2Ship = PlayerController.LocalPlayer.Position - Camera.main.transform.position;
            var soc2Ship = PlayerController.LocalPlayer.Position - this.transform.position;
            return Vector3.Dot(cam2Ship, soc2Ship) > 0;
        }
    }

    public class SlotMarkerData : MarkerData { }
}